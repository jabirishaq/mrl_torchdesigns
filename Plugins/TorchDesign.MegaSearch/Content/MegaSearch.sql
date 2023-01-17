CREATE PROCEDURE  [dbo].[Megasearch] (
	-- Add the parameters for the stored procedure here
	@Keywords nvarchar(4000) = null,
	@ProductSearch bit = 0,
	@SearchDescriptions bit = 0,
	@SearchSku bit = 0,
	@SearchPartNo bit=0,
	@SearchProductTags bit = 0,
	@SearchVideoTags bit =0,
	@CategorySearch bit = 0,
	@CategorySearchDescriptions bit = 0,
	@ManufacturerSearch bit = 0,
	@ManufacturerSearchDescriptions bit = 0,
	@BlogpostSearch bit = 0,
	@BlogpostSearchDescription bit = 0,
	@NewsSearch bit = 0,
	@NewsSearchDescriptions bit = 0,
	@UseFullTextSearch  bit = 0,
	@LanguageId			int = 0,
	@FullTextMode		int = 0 --0 - using CONTAINS with <prefix_term>, 5 - using CONTAINS
	)
AS
BEGIN
	CREATE TABLE #KeywordResult
	(
		EntityId int NOT NULL,
		EntityType int NOT NULL,
		EntityPriority int NOT NULL
	)
		DECLARE
		@SearchKeywords bit,
		@sql nvarchar(max),
		@sql2 nvarchar(max),
		@sql3 nvarchar(max),
		@sql4 nvarchar(max),
		@sql5 nvarchar(max),
		@sql6 nvarchar(max),
		@sql7 nvarchar(max),
		@sql8 nvarchar(max),
		@sql9 nvarchar(max),
		@sql10 nvarchar(max),
		@sql_orderby nvarchar(max),
		@PriorityKeyword nvarchar(max)

	SET NOCOUNT ON
	
	--filter by keywords
	SET @Keywords = isnull(@Keywords, '')
	SET @Keywords = rtrim(ltrim(@Keywords))

	
	IF ISNULL(@Keywords, '') != ''
	BEGIN
		SET @SearchKeywords = 1
		
		IF @UseFullTextSearch = 1
		BEGIN
			--remove wrong chars (' ")
			SET @Keywords = REPLACE(@Keywords, '''', '')
			SET @Keywords = REPLACE(@Keywords, '"', '')
			
			--full-text search
			IF @FullTextMode = 0 
			BEGIN
				--0 - using CONTAINS with <prefix_term>
				SET @Keywords = ' "' + @Keywords + '*" '
			END
			ELSE
			BEGIN
				--5 - using CONTAINS and OR with <prefix_term>
				--10 - using CONTAINS and AND with <prefix_term>

				--clean multiple spaces
				WHILE CHARINDEX('  ', @Keywords) > 0 
					SET @Keywords = REPLACE(@Keywords, '  ', ' ')

				DECLARE @concat_term nvarchar(100)				
				IF @FullTextMode = 5 --5 - using CONTAINS and OR with <prefix_term>
				BEGIN
					SET @concat_term = 'OR'
				END 
				IF @FullTextMode = 10 --10 - using CONTAINS and AND with <prefix_term>
				BEGIN
					SET @concat_term = 'AND'
				END

				--now let's build search string
				declare @fulltext_keywords nvarchar(4000)
				set @fulltext_keywords = N''
				declare @index int		
		
				set @index = CHARINDEX(' ', @Keywords, 0)

				-- if index = 0, then only one field was passed
				IF(@index = 0)
					set @fulltext_keywords = ' "' + @Keywords + '*" '
				ELSE
				BEGIN		
					DECLARE @first BIT
					SET  @first = 1			
					WHILE @index > 0
					BEGIN
						IF (@first = 0)
							SET @fulltext_keywords = @fulltext_keywords + ' ' + @concat_term + ' '
						ELSE
							SET @first = 0

						SET @fulltext_keywords = @fulltext_keywords + '"' + SUBSTRING(@Keywords, 1, @index - 1) + '*"'					
						SET @Keywords = SUBSTRING(@Keywords, @index + 1, LEN(@Keywords) - @index)						
						SET @index = CHARINDEX(' ', @Keywords, 0)
					end
					
					-- add the last field
					IF LEN(@fulltext_keywords) > 0
						SET @fulltext_keywords = @fulltext_keywords + ' ' + @concat_term + ' ' + '"' + SUBSTRING(@Keywords, 1, LEN(@Keywords)) + '*"'	
				END
				SET @Keywords = @fulltext_keywords
			END
		END
		ELSE
		BEGIN
			--usual search by PATINDEX
			SET @PriorityKeyword=@Keywords
			SET @Keywords = '%' + @Keywords + '%'
		END
		PRINT @Keywords
		PRINT @UseFullTextSearch
		--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in Product Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
		IF @ProductSearch=1
		BEGIN
		--product name
		SET @sql = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],EntityPriority)
		SELECT p.Id,1,1
		FROM Product p with (NOLOCK)
		WHERE p.Deleted = 0 AND p.Published = 1 AND '
		IF @UseFullTextSearch = 1
			SET @sql = @sql + 'CONTAINS(p.[Name], @Keywords) '
		ELSE
			SET @sql = @sql + 'PATINDEX(@Keywords, p.[Name]) > 0 '


		--localized product name
		SET @sql = @sql + '
		UNION
		SELECT lp.EntityId,1,1
		FROM LocalizedProperty lp with (NOLOCK)
		WHERE
			lp.LocaleKeyGroup = N''Product''
			AND lp.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
			AND lp.LocaleKey = N''Name'''
		IF @UseFullTextSearch = 1
			SET @sql = @sql + ' AND CONTAINS(lp.[LocaleValue], @Keywords) '
		ELSE
			SET @sql = @sql + ' AND PATINDEX(@Keywords, lp.[LocaleValue]) > 0 '
	

		IF @SearchDescriptions = 1
		BEGIN
			--product short description
			SET @sql = @sql + '
			INSERT INTO #KeywordResult ([EntityId],[EntityType],EntityPriority)
			SELECT p.Id,1,1
			FROM Product p with (NOLOCK)
			WHERE  p.Deleted = 0  AND p.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(p.[ShortDescription], @Keywords) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, p.[ShortDescription]) > 0 '


			--product full description
			SET @sql = @sql + '
			UNION
			SELECT p.Id,1,1
			FROM Product p with (NOLOCK)
			WHERE  p.Deleted = 0  AND p.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(p.[FullDescription], @Keywords) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, p.[FullDescription]) > 0 '



			--localized product short description
			SET @sql = @sql + '
			UNION
			SELECT lp.EntityId,1,1
			FROM LocalizedProperty lp with (NOLOCK)
			WHERE
				lp.LocaleKeyGroup = N''Product''
				AND lp.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND lp.LocaleKey = N''ShortDescription'''
			IF @UseFullTextSearch = 1
				SET @sql = @sql + ' AND CONTAINS(lp.[LocaleValue], @Keywords) '
			ELSE
				SET @sql = @sql + ' AND PATINDEX(@Keywords, lp.[LocaleValue]) > 0 '
				

			--localized product full description
			SET @sql = @sql + '
			UNION
			SELECT lp.EntityId,1,1
			FROM LocalizedProperty lp with (NOLOCK)
			WHERE
				lp.LocaleKeyGroup = N''Product''
				AND lp.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND lp.LocaleKey = N''FullDescription'''
			IF @UseFullTextSearch = 1
				SET @sql = @sql + ' AND CONTAINS(lp.[LocaleValue], @Keywords) '
			ELSE
				SET @sql = @sql + ' AND PATINDEX(@Keywords, lp.[LocaleValue]) > 0 '
		END

		--SKU
		IF @SearchSku = 1
		BEGIN
			SET @sql = @sql + '
			UNION
			SELECT p.Id,1,1
			FROM Product p with (NOLOCK)
			WHERE  p.Deleted = 0  AND p.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(p.[Sku], @Keywords) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, p.[Sku]) > 0 '
		END
		IF @SearchPartNo = 1
		BEGIN
			SET @sql = @sql + '
			UNION
			SELECT p.Id,1,1
			FROM Product p with (NOLOCK)
			WHERE  p.Deleted = 0  AND p.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(p.[ManufacturerPartNumber], @Keywords) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, p.[ManufacturerPartNumber]) > 0 '
		END

		IF @SearchProductTags = 1
		BEGIN
			--product tag
			SET @sql = @sql + '
			UNION
			Select p.Id,1,1 From Product p Where Deleted=0  AND p.Published = 1 And Id In(SELECT pptm.Product_Id
			FROM Product_ProductTag_Mapping pptm with(NOLOCK) INNER JOIN ProductTag pt with(NOLOCK) ON pt.Id = pptm.ProductTag_Id
			WHERE '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(pt.[Name], @Keywords)) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, pt.[Name]) > 0) '

			--localized product tag
			SET @sql = @sql + '
			UNION
			SELECT pptm.Product_Id,1,1
			FROM LocalizedProperty lp with (NOLOCK) INNER JOIN Product_ProductTag_Mapping pptm with(NOLOCK) ON lp.EntityId = pptm.ProductTag_Id
			WHERE
				lp.LocaleKeyGroup = N''ProductTag''
				AND lp.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND lp.LocaleKey = N''Name'''
			IF @UseFullTextSearch = 1
				SET @sql = @sql + ' AND CONTAINS(lp.[LocaleValue], @Keywords) '
			ELSE
				SET @sql = @sql + ' AND PATINDEX(@Keywords, lp.[LocaleValue]) > 0 '
		END
		
		IF (Not EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TD_SupportTopicSupportCategory'))
		BEGIN
		IF @SearchVideoTags = 1
		BEGIN
			--Video tag
			SET @sql = @sql + '
			UNION
			Select p.Id,1,1 From Product p Where Deleted=0  AND p.Published = 1 And Id In(SELECT pvtm.ProductId
			FROM Td_Product_Videos_Mapping pvtm with(NOLOCK) WHERE '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(pvtm.[TagName], @Keywords) OR CONTAINS(pvtm.[VideoTitle], @Keywords) OR CONTAINS(pvtm.[VideoDescription], @Keywords)) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, pvtm.[TagName]) > 0 OR PATINDEX(@Keywords, pvtm.[VideoTitle]) > 0 OR PATINDEX(@Keywords, pvtm.[VideoDescription]) > 0)'


			----video title
			--SET @sql = @sql + '
			--UNION
			--SELECT p.ProductId,1
			--FROM Td_Product_Videos_Mapping p with(NOLOCK) WHERE '
			--IF @UseFullTextSearch = 1
			--	SET @sql = @sql + 'CONTAINS(p.[VideoTitle], @Keywords) '
			--ELSE
			--	SET @sql = @sql + 'PATINDEX(@Keywords, p.[VideoTitle]) > 0 '


			----video full description
			--SET @sql = @sql + '
			--UNION
			--SELECT p.ProductId,1
			--FROM Td_Product_Videos_Mapping p with(NOLOCK) WHERE '
			--IF @UseFullTextSearch = 1
			--	SET @sql = @sql + 'CONTAINS(p.[VideoDescription], @Keywords) '
			--ELSE
			--	SET @sql = @sql + 'PATINDEX(@Keywords, p.[VideoDescription]) > 0 '



			--localized Video tag
			--SET @sql = @sql + '
			--UNION
			--SELECT pvtm.Product_Id,1
			--FROM LocalizedProperty lp with (NOLOCK) INNER JOIN Td_Product_Videos_Mapping pvtm with(NOLOCK) ON lp.EntityId = pvtm.ProductTag_Id
			--WHERE
			--	lp.LocaleKeyGroup = N''ProductTag''
			--	AND lp.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
			--	AND lp.LocaleKey = N''Name'''
			--IF @UseFullTextSearch = 1
			--	SET @sql = @sql + ' AND CONTAINS(lp.[LocaleValue], @Keywords) '
			--ELSE
			--	SET @sql = @sql + ' AND PATINDEX(@Keywords, lp.[LocaleValue]) > 0 '
		END
		Print @sql
		EXEC sp_executesql @sql, N'@Keywords nvarchar(4000)', @Keywords
		END
		ENd
		--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in Category Table----------------------------------------
	----////////////////////////////////////////////////////////////////////////////////////////////

		--Category name
		IF @CategorySearch=1
		BEGIN
		SET @sql2 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT c.Id,2,1
		FROM Category c with (NOLOCK)
		WHERE c.Deleted = 0  AND c.Published = 1 AND '
		IF @UseFullTextSearch = 1
			SET @sql2 = @sql2 + 'CONTAINS(c.[Name], @Keywords) '
		ELSE
			SET @sql2 = @sql2 + 'PATINDEX(@Keywords, c.[Name]) > 0 '

		--localized Category name
		SET @sql2 = @sql2 + '
		UNION
		SELECT lc.EntityId,2,1
		FROM LocalizedProperty lc with (NOLOCK)
		WHERE
			lc.LocaleKeyGroup = N''Category''
			AND lc.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
			AND lc.LocaleKey = N''Name'''
		IF @UseFullTextSearch = 1
			SET @sql2 = @sql2 + ' AND CONTAINS(lc.[LocaleValue], @Keywords) '
		ELSE
			SET @sql2 = @sql2 + ' AND PATINDEX(@Keywords, lc.[LocaleValue]) > 0 '
	

		IF @CategorySearchDescriptions = 1
		BEGIN
			--product description

			SET @sql2 = @sql2 + '
			UNION
			SELECT c.Id,2,1
			FROM Category c with (NOLOCK)
			WHERE c.Deleted = 0 AND c.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql2 = @sql2 + 'CONTAINS(c.[Description], @Keywords) '
			ELSE
				SET @sql2 = @sql2 + 'PATINDEX(@Keywords, c.[Description]) > 0 '

		--localized product short description
			SET @sql2 = @sql2 + '
			UNION
			SELECT lc.EntityId,2,1
			FROM LocalizedProperty lc with (NOLOCK)
			WHERE
				lc.LocaleKeyGroup = N''Category''
				AND lc.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND lc.LocaleKey = N''Description'''
			IF @UseFullTextSearch = 1
				SET @sql2 = @sql2 + ' AND CONTAINS(lc.[LocaleValue], @Keywords) '
			ELSE
				SET @sql2 = @sql2 + ' AND PATINDEX(@Keywords, lc.[LocaleValue]) > 0 '
	
		END
		Print @sql2
		EXEC sp_executesql @sql2, N'@Keywords nvarchar(4000)', @Keywords

		END
--			--	--////////////////////////////////////////////////////////////////////////////////////////////
--		-------------------------------------Check in Manufacturer Table----------------------------------------
--		----////////////////////////////////////////////////////////////////////////////////////////////
		IF @ManufacturerSearch=1
		BEGIN
	--Manufacturer name
		SET @sql3 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT m.Id,3,1
		FROM Manufacturer m with (NOLOCK)
		WHERE m.Deleted = 0 AND m.Published = 1 AND '
		IF @UseFullTextSearch = 1
			SET @sql3 = @sql3 + 'CONTAINS(m.[Name], @Keywords) '
		ELSE
			SET @sql3 = @sql3 + 'PATINDEX(@Keywords, m.[Name]) > 0 '


		--localized Manufacturer name
		SET @sql3 = @sql3 + '
		UNION
		SELECT lm.EntityId,3,1
		FROM LocalizedProperty lm with (NOLOCK)
		WHERE
			lm.LocaleKeyGroup = N''Manufacturer''
			AND lm.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
			AND lm.LocaleKey = N''Name'''
		IF @UseFullTextSearch = 1
			SET @sql3 = @sql3 + ' AND CONTAINS(lm.[LocaleValue], @Keywords) '
		ELSE
			SET @sql3 = @sql3 + ' AND PATINDEX(@Keywords, lm.[LocaleValue]) > 0 '
	

		IF @ManufacturerSearchDescriptions = 1
		BEGIN
			--Manufacturer description

			SET @sql3 = @sql3 + '
			UNION
			SELECT m.Id,3,1
			FROM Manufacturer m with (NOLOCK)
			WHERE m.Deleted = 0 AND m.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql3 = @sql3 + 'CONTAINS(m.[Description], @Keywords) '
			ELSE
				SET @sql3 = @sql3 + 'PATINDEX(@Keywords, m.[Description]) > 0 '

			--localized Manufacturer description
			SET @sql3 = @sql3 + '
			UNION
			SELECT lm.EntityId,3,1
			FROM LocalizedProperty lm with (NOLOCK)
			WHERE
				lm.LocaleKeyGroup = N''Manufacturer''
				AND lm.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND lm.LocaleKey = N''Description'''
			IF @UseFullTextSearch = 1
				SET @sql3 = @sql3 + ' AND CONTAINS(lm.[LocaleValue], @Keywords) '
			ELSE
				SET @sql3 = @sql3 + ' AND PATINDEX(@Keywords, lm.[LocaleValue]) > 0 '
	
		END
		Print @sql3
		EXEC sp_executesql @sql3, N'@Keywords nvarchar(4000)', @Keywords
		END
			--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in BlogPost Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
		IF @BlogpostSearch = 1 
		BEGIN
		--BlogPost Title
		SET @sql4 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT b.Id,4,1
		FROM BlogPost b with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql4 = @sql4 + 'CONTAINS(b.[Title], @Keywords) '
		ELSE
			SET @sql4 = @sql4 + 'PATINDEX(@Keywords, b.[Title]) > 0 '

		--localized BlogPost Title
		SET @sql4 = @sql4 + '
		UNION
		SELECT lb.EntityId,4,1
		FROM LocalizedProperty lb with (NOLOCK)
		WHERE
			lb.LocaleKeyGroup = N''BlogPost''
			AND lb.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
			AND lb.LocaleKey = N''Title'''
		IF @UseFullTextSearch = 1
			SET @sql4 = @sql4 + ' AND CONTAINS(lb.[LocaleValue], @Keywords) '
		ELSE
			SET @sql4 = @sql4 + ' AND PATINDEX(@Keywords, lb.[LocaleValue]) > 0 '
	
		IF @BlogpostSearchDescription = 1
		BEGIN
			--BlogPost Body

			SET @sql4 = @sql4 + '
			UNION
			SELECT b.Id,4,1
			FROM BlogPost b with (NOLOCK)
			WHERE '
			IF @UseFullTextSearch = 1
				SET @sql4 = @sql4 + 'CONTAINS(b.[Body], @Keywords) '
			ELSE
				SET @sql4 = @sql4 + 'PATINDEX(@Keywords, b.[Body]) > 0 '

			--localized Manufacturer description
			SET @sql4 = @sql4 + '
			UNION
			SELECT lb.EntityId,4,1
			FROM LocalizedProperty lb with (NOLOCK)
			WHERE
				lb.LocaleKeyGroup = N''BlogPost''
				AND lb.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND lb.LocaleKey = N''Body'''
			IF @UseFullTextSearch = 1
				SET @sql4 = @sql4 + ' AND CONTAINS(lb.[LocaleValue], @Keywords) '
			ELSE
				SET @sql4 = @sql4 + ' AND PATINDEX(@Keywords, lb.[LocaleValue]) > 0 '
	
		END
		Print @sql4
		EXEC sp_executesql @sql4, N'@Keywords nvarchar(4000)', @Keywords

	END

			--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in News Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
		IF @NewsSearch = 1
		BEGIN
		--News Title
		SET @sql5 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT n.Id,5,1
		FROM News n with (NOLOCK)
		WHERE n.Published = 1 AND '
		IF @UseFullTextSearch = 1
			SET @sql5 = @sql5 + 'CONTAINS(n.[Title], @Keywords) '
		ELSE
			SET @sql5 = @sql5 + 'PATINDEX(@Keywords, n.[Title]) > 0 '


		--localized product Title
		SET @sql5 = @sql5 + '
		UNION
		SELECT ln.EntityId,5,1
		FROM LocalizedProperty ln with (NOLOCK)
		WHERE
			ln.LocaleKeyGroup = N''News''
			AND ln.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
			AND ln.LocaleKey = N''Title'''
		IF @UseFullTextSearch = 1
			SET @sql5 = @sql5 + ' AND CONTAINS(ln.[LocaleValue], @Keywords) '
		ELSE
			SET @sql5 = @sql5 + ' AND PATINDEX(@Keywords, ln.[LocaleValue]) > 0 '
	

		IF @NewsSearchDescriptions = 1
		BEGIN
			--News Short
			SET @sql5 = @sql5 + '
			UNION
			SELECT n.Id,5,1
			FROM News n with (NOLOCK)
			WHERE n.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql5 = @sql5 + 'CONTAINS(n.[Short], @Keywords) '
			ELSE
				SET @sql5 = @sql5 + 'PATINDEX(@Keywords, n.[Short]) > 0 '


			--product full description
			SET @sql5 = @sql5 + '
			UNION
			SELECT n.Id,5,1
			FROM News n with (NOLOCK)
			WHERE n.Published = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql5 = @sql5 + 'CONTAINS(n.[Full], @Keywords) '
			ELSE
				SET @sql5 = @sql5 + 'PATINDEX(@Keywords, n.[Full]) > 0 '



			--localized News short description and full description
			SET @sql5 = @sql5 + '
			UNION
			SELECT ln.EntityId,5,1
			FROM LocalizedProperty ln with (NOLOCK)
			WHERE
				ln.LocaleKeyGroup = N''News''
				AND ln.LanguageId = ' + ISNULL(CAST(@LanguageId AS nvarchar(max)), '0') + '
				AND (ln.LocaleKey = N''Short'' OR ln.LocaleKey = N''Full'')'
			IF @UseFullTextSearch = 1
				SET @sql5 = @sql5 + ' AND CONTAINS(ln.[LocaleValue], @Keywords) '
			ELSE
				SET @sql5 = @sql5 + ' AND PATINDEX(@Keywords, ln.[LocaleValue]) > 0 '
			
		END
		Print @sql5
		EXEC sp_executesql @sql5, N'@Keywords nvarchar(4000)', @Keywords
		END

-----------------------------------------------------------------------------------------------------------------		
--	--////////////////////////////////////////////////////////////////////////////////////////////////////////
		--------///*************///--------------Check in SupportTable---------------///***************///-------
		----////////////////////////////////////////////////////////////////////////////////////////////
-----------------------------------------------------------------------------------------------------------------



		IF (EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'TD_SupportTopicSupportCategory'))
BEGIN

--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportCategory Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
		
		----SupportCategory name
		--SET @sql6 = '
		--INSERT INTO #KeywordResult ([EntityId],[EntityType],EntityPriority)
		--SELECT p.Id,6,1
		--FROM TD_SupportTopicSupportCategory p with (NOLOCK)
		--WHERE p.IsActive = 1 AND '
		--IF @UseFullTextSearch = 1
		--	SET @sql6 = @sql6 + 'CONTAINS(p.[Title], @Keywords) '
		--ELSE
		--	SET @sql6 = @sql6 + 'PATINDEX(@Keywords, p.[Title]) > 0 '
		--	--SupportCategory short description
		--	SET @sql6 = @sql6 + '
		--	INSERT INTO #KeywordResult ([EntityId],[EntityType],EntityPriority)
		--	SELECT p.Id,6,1
		--	FROM TD_SupportTopicSupportCategory p with (NOLOCK)
		--	WHERE p.IsActive = 1 AND '
		--	IF @UseFullTextSearch = 1
		--		SET @sql6 = @sql6 + 'CONTAINS(p.[Description], @Keywords) '
		--	ELSE
		--		SET @sql6 = @sql6 + 'PATINDEX(@Keywords, p.[Description]) > 0 '

		--Print @sql6
		--EXEC sp_executesql @sql6, N'@Keywords nvarchar(4000)', @Keywords
		
		--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportTopic Table----------------------------------------
	----////////////////////////////////////////////////////////////////////////////////////////////

		--SupportTopic name
	
		SET @sql7 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT c.Id,7,1
		FROM TD_SupportTopic c with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql7 = @sql7 + 'CONTAINS(c.[Title], @Keywords) '
		ELSE
			SET @sql7 = @sql7 + 'PATINDEX(@Keywords, c.[Title]) > 0 '

			--SupportTopic description

			SET @sql7 = @sql7 + '
			UNION
			SELECT c.Id,7,1
			FROM TD_SupportTopic c with (NOLOCK)
			WHERE '
			IF @UseFullTextSearch = 1
				SET @sql7 = @sql7 + 'CONTAINS(c.[Description], @Keywords) '
			ELSE
				SET @sql7 = @sql7 + 'PATINDEX(@Keywords, c.[Description]) > 0 '

		Print @sql7
		EXEC sp_executesql @sql7, N'@Keywords nvarchar(4000)', @Keywords


		--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportTopicStep Table----------------------------------------
	----////////////////////////////////////////////////////////////////////////////////////////////

		--SupportStep name
	
		SET @sql8 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT c.SupportTopicId,7,1
		FROM TD_SupportTopicStep c with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql8 = @sql8 + 'CONTAINS(c.[Title], @Keywords) '
		ELSE
			SET @sql8 = @sql8 + 'PATINDEX(@Keywords, c.[Title]) > 0 '

			--SupportStep description

			SET @sql8 = @sql8 + '
			UNION
			SELECT c.SupportTopicId,7,1
			FROM TD_SupportTopicStep c with (NOLOCK)
			WHERE '
			IF @UseFullTextSearch = 1
				SET @sql8 = @sql8 + 'CONTAINS(c.[Description], @Keywords) '
			ELSE
				SET @sql8 = @sql8 + 'PATINDEX(@Keywords, c.[Description]) > 0 '

		Print @sql8
		EXEC sp_executesql @sql8, N'@Keywords nvarchar(4000)', @Keywords


		
--			--	--////////////////////////////////////////////////////////////////////////////////////////////
--		-------------------------------------Check in SupportVideo Table----------------------------------------
--		----////////////////////////////////////////////////////////////////////////////////////////////
		
	--Manufacturer name

		SET @sql9 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT pvtm.Id,8,1
		FROM TD_SupportVideo pvtm with (NOLOCK)
		WHERE '
	IF @UseFullTextSearch = 1
				SET @sql9 = @sql9 + 'CONTAINS(pvtm.[Title], @Keywords) OR CONTAINS(pvtm.[Description], @Keywords) OR CONTAINS(pvtm.[Tag], @Keywords)) '
			ELSE
				SET @sql9 = @sql9 + 'PATINDEX(@Keywords, pvtm.[Title]) > 0 OR PATINDEX(@Keywords, pvtm.[Description]) > 0 OR PATINDEX(@Keywords, pvtm.[Tag]) > 0'

		
		Print @sql9
		EXEC sp_executesql @sql9, N'@Keywords nvarchar(4000)', @Keywords
		
			--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportDownload Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
	
		--BlogPost Title
		SET @sql10 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT b.Id,9,1
		FROM TD_SupportDownloadMapping b with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql10 = @sql10 + 'CONTAINS(b.[Title], @Keywords) OR CONTAINS(b.[Description], @Keywords) '
		ELSE
			SET @sql10 = @sql10 + 'PATINDEX(@Keywords, b.[Title]) > 0 OR PATINDEX(@Keywords, b.[Description]) > 0'

		Print @sql10
		EXEC sp_executesql @sql10, N'@Keywords nvarchar(4000)', @Keywords

		   
END
------------------------------------------------------------------------------------------------------------------

			Update #KeywordResult SET [EntityPriority]= 0 WHERE [EntityId] In( SELECT p.Id
		FROM Product p with (NOLOCK)
		WHERE p.Deleted = 0 AND p.Published = 1 AND p.[ManufacturerPartNumber]=@PriorityKeyword)
	END
	ELSE
	BEGIN
		SET @SearchKeywords = 0
	END
	
	SELECT * FROM #KeywordResult ORDER BY [EntityPriority]
	DROP TABLE #KeywordResult
	
END