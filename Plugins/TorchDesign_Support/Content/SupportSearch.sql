Create PROCEDURE  [dbo].[SupportSearch] (
	-- Add the parameters for the stored procedure here
	@Keywords nvarchar(4000) = null,
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
		-------------------------------------Check in SupportCategory Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
		
		--SupportCategory name
		SET @sql = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],EntityPriority)
		SELECT p.Id,1,1
		FROM TD_SupportTopicSupportCategory p with (NOLOCK)
		WHERE p.IsActive = 1 AND '
		IF @UseFullTextSearch = 1
			SET @sql = @sql + 'CONTAINS(p.[Title], @Keywords) '
		ELSE
			SET @sql = @sql + 'PATINDEX(@Keywords, p.[Title]) > 0 '
			--SupportCategory short description
			SET @sql = @sql + '
			INSERT INTO #KeywordResult ([EntityId],[EntityType],EntityPriority)
			SELECT p.Id,1,1
			FROM TD_SupportTopicSupportCategory p with (NOLOCK)
			WHERE p.IsActive = 1 AND '
			IF @UseFullTextSearch = 1
				SET @sql = @sql + 'CONTAINS(p.[Description], @Keywords) '
			ELSE
				SET @sql = @sql + 'PATINDEX(@Keywords, p.[Description]) > 0 '

		Print @sql
		EXEC sp_executesql @sql, N'@Keywords nvarchar(4000)', @Keywords
		
		--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportTopic Table----------------------------------------
	----////////////////////////////////////////////////////////////////////////////////////////////

		--SupportTopic name
	
		SET @sql2 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT c.Id,2,1
		FROM TD_SupportTopic c with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql2 = @sql2 + 'CONTAINS(c.[Title], @Keywords) '
		ELSE
			SET @sql2 = @sql2 + 'PATINDEX(@Keywords, c.[Title]) > 0 '

			--SupportTopic description

			SET @sql2 = @sql2 + '
			UNION
			SELECT c.Id,2,1
			FROM TD_SupportTopic c with (NOLOCK)
			WHERE '
			IF @UseFullTextSearch = 1
				SET @sql2 = @sql2 + 'CONTAINS(c.[Description], @Keywords) '
			ELSE
				SET @sql2 = @sql2 + 'PATINDEX(@Keywords, c.[Description]) > 0 '

		Print @sql2
		EXEC sp_executesql @sql2, N'@Keywords nvarchar(4000)', @Keywords

		--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportTopicStep Table----------------------------------------
	----////////////////////////////////////////////////////////////////////////////////////////////

		--SupportStep name
	
		SET @sql2 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT c.SupportTopicId,2,1
		FROM TD_SupportTopicStep c with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql2 = @sql2 + 'CONTAINS(c.[Title], @Keywords) '
		ELSE
			SET @sql2 = @sql2 + 'PATINDEX(@Keywords, c.[Title]) > 0 '

			--SupportStep description

			SET @sql2 = @sql2 + '
			UNION
			SELECT c.SupportTopicId,2,1
			FROM TD_SupportTopicStep c with (NOLOCK)
			WHERE '
			IF @UseFullTextSearch = 1
				SET @sql2 = @sql2 + 'CONTAINS(c.[Description], @Keywords) '
			ELSE
				SET @sql2 = @sql2 + 'PATINDEX(@Keywords, c.[Description]) > 0 '

		Print @sql2
		EXEC sp_executesql @sql2, N'@Keywords nvarchar(4000)', @Keywords


		
--			--	--////////////////////////////////////////////////////////////////////////////////////////////
--		-------------------------------------Check in SupportVideo Table----------------------------------------
--		----////////////////////////////////////////////////////////////////////////////////////////////
		
	--Manufacturer name

		SET @sql3 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT pvtm.Id,3,1
		FROM TD_SupportVideo pvtm with (NOLOCK)
		WHERE '
	IF @UseFullTextSearch = 1
				SET @sql3 = @sql3 + 'CONTAINS(pvtm.[Title], @Keywords) OR CONTAINS(pvtm.[Description], @Keywords) OR CONTAINS(pvtm.[Tag], @Keywords)) '
			ELSE
				SET @sql3 = @sql3 + 'PATINDEX(@Keywords, pvtm.[Title]) > 0 OR PATINDEX(@Keywords, pvtm.[Description]) > 0 OR PATINDEX(@Keywords, pvtm.[Tag]) > 0'

		
		Print @sql3
		EXEC sp_executesql @sql3, N'@Keywords nvarchar(4000)', @Keywords
		
			--	--////////////////////////////////////////////////////////////////////////////////////////////
		-------------------------------------Check in SupportDownload Table----------------------------------------
		----////////////////////////////////////////////////////////////////////////////////////////////
	
		--BlogPost Title
		SET @sql4 = '
		INSERT INTO #KeywordResult ([EntityId],[EntityType],[EntityPriority])		
		SELECT b.Id,4,1
		FROM TD_SupportDownloadMapping b with (NOLOCK)
		WHERE '
		IF @UseFullTextSearch = 1
			SET @sql4 = @sql4 + 'CONTAINS(b.[Title], @Keywords) OR CONTAINS(b.[Description], @Keywords) '
		ELSE
			SET @sql4 = @sql4 + 'PATINDEX(@Keywords, b.[Title]) > 0 OR PATINDEX(@Keywords, b.[Description]) > 0'

		Print @sql4
		EXEC sp_executesql @sql4, N'@Keywords nvarchar(4000)', @Keywords


			Update #KeywordResult SET [EntityPriority]= 0 WHERE [EntityId] In( SELECT p.Id
		FROM TD_SupportTopic p with (NOLOCK))
	END
	ELSE
	BEGIN
		SET @SearchKeywords = 0
	END
	
	SELECT * FROM #KeywordResult ORDER BY [EntityPriority]
	DROP TABLE #KeywordResult
	
END
