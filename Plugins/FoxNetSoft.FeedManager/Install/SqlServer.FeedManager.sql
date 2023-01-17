declare @feedid int, @languageId int, @storeid int, @currencyid int
select top 1 @languageId=Id from [Language] L where L.Published=1 order by L.DisplayOrder
select top 1 @storeid=Id from Store S order by S.DisplayOrder
select top 1 @currencyid=Id from Currency C order by C.DisplayOrder

if not exists(select * from FNS_FeedRec where TypeFeedId =1)
begin
	insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
		Name,EncodingId,AdminComment,ExportFileName,IsActive,
		ProductPictureSize,FTPSettingsXML,
		[Compression],DoNotExportNewProducts,
		CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
	values (1, @storeid, @languageId, @currencyid,
		'Google',1,'Google feed','google',0,
		400,'',
		0,0,
		'>',1,1,1,0)
	set @feedid=SCOPE_IDENTITY()
	insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
		ConditionValueId,ConditionValue)
	values (@feedid,1,1,3,3,0,'')

	if not exists(select * from FNS_FeedRec where TypeFeedId =20)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (20,@storeid, @languageId, @currencyid, 
			'AmazonAds',1,'AmazonAds feed','amazonads',0,
			500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end

	if not exists(select * from FNS_FeedRec where TypeFeedId =22)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (22,@storeid, @languageId, @currencyid, 
			'Amazon Inventory',1,'Amazon Inventory feed','amazoninventory',0,
			500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =23)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (23,@storeid, @languageId, @currencyid, 
			'Amazon Pricing',1,'Amazon Pricing feed','amazonpricing',0,
			500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =24)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (24,@storeid, @languageId, @currencyid, 
			'Amazon Image',1,'Amazon Image feed','amazonimage',0,
			500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =30)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (30, @storeid, @languageId, @currencyid,
			'PriceGrabber',1,'PriceGrabber feed','pricegrabber',0,
			400,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =170)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			MerchantId,ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (170,@storeid, @languageId, @currencyid, 
			'Ebay (Product Inventory)',1,'Ebay Product Inventory feed','ebayproductinventory',0,
			'SiteID=US|Country=US|Currency=USD|Version=745',500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =171)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			MerchantId,ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (171,@storeid, @languageId, @currencyid, 
			'Ebay (Basic Template)',1,'Ebay Basic Template feed','ebaybasictemplate',0,
			'SiteID=US|Country=US|Currency=USD|Version=745',1000,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =40)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (40, @storeid, @languageId, @currencyid, 
			'Twenga',1,'Twenga feed','twenga',0,
			500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =90)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (90,@storeid, @languageId, @currencyid, 
			'Shopping.com',1,'Shopping.com feed','shoppingcom',0,
			500,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =110)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (110,@storeid, @languageId, @currencyid, 
			'LeGuide',1,'LeGuide feed','leguide',0,
			340,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
	if not exists(select * from FNS_FeedRec where TypeFeedId =120)
	begin
		insert into FNS_FeedRec (TypeFeedId, StoreId, LanguageId,CurrencyId,
			Name,EncodingId,AdminComment,ExportFileName,IsActive,
			ProductPictureSize,FTPSettingsXML,
			[Compression],DoNotExportNewProducts,
			CategoryPathDelimiter,DecimalDelimiterId,PriceFormatId,DatetimeFormatId,PriceMarkup)
		values (120,@storeid, @languageId, @currencyid, 
			'Shopzilla',1,'Shopzilla feed','shopzilla',0,
			340,'',
			0,0,
			'>',1,1,1,0)
		set @feedid=SCOPE_IDENTITY()
		insert into FNS_FeedCondition (FeedId,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,
			ConditionValueId,ConditionValue)
		values (@feedid,1,1,3,3,0,'')
	end
end
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FNS_FeedManager_ProductLoadAll]') AND type in (N'P', N'PC'))
DROP PROCEDURE [FNS_FeedManager_ProductLoadAll]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FNS_FeedManager_ProductLoadAll]
(
	@FeedId		int,	
	@AllowedCustomerRoleIds	nvarchar(MAX) = null,	--a list of customer role IDs (comma-separated list) for which a product should be shown (if a subjet to ACL)
	@PageIndex			int = 0, 
	@PageSize			int = 2147483644
)
AS
BEGIN
SET NOCOUNT ON
DECLARE @StoreId int,@LanguageId int
select @StoreId=StoreId,@LanguageId=LanguageId from FNS_FeedRec WITH (NOLOCK) where Id=@FeedId
if @LanguageId>0
begin	
	declare @countLanguage int
	set @countLanguage=0
	select @countLanguage=count(*) from [Language] L WITH (NOLOCK) where L.Published=1
	if @countLanguage=1
	begin
		set @LanguageId=0
	end
end

DECLARE @sql nvarchar(max)

	CREATE TABLE #ExportProductsTmp
	(
		[ProductId] int NOT NULL
	)
	
	--filter by customer role IDs (access control list)
	SET @AllowedCustomerRoleIds = isnull(@AllowedCustomerRoleIds, '')	
	CREATE TABLE #FilteredCustomerRoleIds
	(
		CustomerRoleId int not null
	)
	INSERT INTO #FilteredCustomerRoleIds (CustomerRoleId)
	SELECT CAST(data as int) FROM [nop_splitstring_to_table](@AllowedCustomerRoleIds, ',')

		--product name
		SET @sql = 'INSERT INTO #ExportProductsTmp (ProductId)
		SELECT p.Id
		FROM Product p with (NOLOCK)
		WHERE (p.VisibleIndividually = 1 or p.ParentGroupedProductId!=0)
			AND p.ProductTypeId=5
			AND p.Published = 1
			AND p.Deleted = 0
			AND p.Id in (select ProductId from Product_Category_Mapping WITH (NOLOCK)) 
			AND (getutcdate() BETWEEN ISNULL(p.AvailableStartDateTimeUtc, ''1/1/1900'') and ISNULL(p.AvailableEndDateTimeUtc, ''1/1/2999''))'
	
	
	--show hidden and ACL
		SET @sql = @sql + '
		AND (p.SubjectToAcl = 0 OR EXISTS (
			SELECT 1 FROM #FilteredCustomerRoleIds [fcr]
			WHERE
				[fcr].CustomerRoleId IN (
					SELECT [acl].CustomerRoleId
					FROM [AclRecord] acl with (NOLOCK)
					WHERE [acl].EntityId = p.Id AND [acl].EntityName = ''Product''
				)
			))'
	
	--show hidden and filter by store
	IF @StoreId > 0
	BEGIN
		SET @sql = @sql + '
		AND (p.LimitedToStores = 0 OR EXISTS (
			SELECT 1 FROM [StoreMapping] sm with (NOLOCK)
			WHERE [sm].EntityId = p.Id AND [sm].EntityName = ''Product'' and [sm].StoreId=' + CAST(@StoreId AS nvarchar(max)) + '
			))'
	END

	EXEC sp_executesql @sql
drop table #FilteredCustomerRoleIds

if exists(select * from FNS_FeedCondition C WITH (NOLOCK) where C.FeedId=@FeedId)
begin
	DECLARE @sqlCondition nvarchar(max)
	declare @FeedConditionId int,@ConditionGroupId int,@ConditionTypeId int,@ConditionPropertyId int,@ConditionOperatorId int,@ConditionValueId int
	DECLARE View_FeedCondition INSENSITIVE CURSOR
	FOR SELECT Id,ConditionGroupId,ConditionTypeId,ConditionPropertyId,ConditionOperatorId,ConditionValueId
	FROM FNS_FeedCondition WITH (NOLOCK)
	where FeedId=@FeedId
	FOR READ ONLY

	OPEN View_FeedCondition
	FETCH NEXT FROM View_FeedCondition into @FeedConditionId,@ConditionGroupId,@ConditionTypeId,@ConditionPropertyId,@ConditionOperatorId,@ConditionValueId
	WHILE @@Fetch_Status=0
	begin
		set @sqlCondition=''
		--Product
		if @ConditionTypeId=1
		begin
			--Category
			if @ConditionPropertyId=1
			begin
				--Equal To
				if @ConditionOperatorId=0
					set @sqlCondition=@sqlCondition+' ProductId not in '
				else	
					set @sqlCondition=@sqlCondition+' ProductId in '
					--Not Equal To
					--@ConditionOperatorId=1
					
				set @sqlCondition=@sqlCondition+' (select SM.ProductId
						from Product_Category_Mapping SM
						where SM.CategoryId='+str(@ConditionValueId)+')'
			end
			--Manufacturer
			if @ConditionPropertyId=2
			begin
				--Equal To
				if @ConditionOperatorId=0
					set @sqlCondition=@sqlCondition+' ProductId not in '
				else	
					set @sqlCondition=@sqlCondition+' ProductId in '
					--Not Equal To
					--@ConditionOperatorId=1
					
				set @sqlCondition=@sqlCondition+' (select SM.ProductId
						from Product_Manufacturer_Mapping SM
						where SM.ManufacturerId='+str(@ConditionValueId)+')'
			end	
			
			if @ConditionPropertyId in (3,4,5,6)
			begin
				set @sqlCondition=@sqlCondition+' ProductId in (select SM.Id
					from Product SM
					where '
				--Price
				if @ConditionPropertyId=3
					set @sqlCondition=@sqlCondition+' Price'
				--Quantity
				if @ConditionPropertyId=4
					set @sqlCondition=@sqlCondition+' StockQuantity'
				--PreOrder
				if @ConditionPropertyId=5
					set @sqlCondition=@sqlCondition+' AvailableForPreOrder'
				--FreeShipping
				if @ConditionPropertyId=6
					set @sqlCondition=@sqlCondition+' IsFreeShipping'

				if @ConditionOperatorId=0
					set @sqlCondition=@sqlCondition+'!='
				if @ConditionOperatorId=1	
					set @sqlCondition=@sqlCondition+'='
				if @ConditionOperatorId=2	
					set @sqlCondition=@sqlCondition+'>='
				if @ConditionOperatorId=3	
					set @sqlCondition=@sqlCondition+'<='
				if @ConditionOperatorId=4	
					set @sqlCondition=@sqlCondition+'>'
				if @ConditionOperatorId=5	
					set @sqlCondition=@sqlCondition+'<'
		
				set @sqlCondition=@sqlCondition+str(@ConditionValueId)+')'
			end	
		end
		--ProductSpecificationAttribute
		if @ConditionTypeId=2
		begin
			--Equal To
			if @ConditionOperatorId=0
				set @sqlCondition=@sqlCondition+' ProductId not in '
			else	
				set @sqlCondition=@sqlCondition+' ProductId in '
				--Not Equal To
				--@ConditionOperatorId=1
				
			set @sqlCondition=@sqlCondition+' (select SM.ProductId
					from Product_SpecificationAttribute_Mapping SM
					where SM.SpecificationAttributeOptionId='+str(@ConditionValueId)+')'
		end
		if len(@sqlCondition)>0
		begin
			SET @sqlCondition='delete from #ExportProductsTmp where '+@sqlCondition
			--print @sqlCondition
			EXEC sp_executesql @sqlCondition
		end	
		FETCH NEXT FROM View_FeedCondition into @FeedConditionId,@ConditionGroupId,@ConditionTypeId,@ConditionPropertyId,@ConditionOperatorId,@ConditionValueId
	end	
	DEALLOCATE View_FeedCondition
end

--Delete Non export products
CREATE TABLE #tmpFeedDefault (CategoryId int, ParentCategoryId int, ProductId int, Levelsort int, FeedCategoryId int, CategoryMap nvarchar(1000) null, DefaultValuesXML nvarchar(max) null, TypeActiveId int)

--FNS_FeedCategory
if exists(select * from FNS_FeedCategory F WITH (NOLOCK) where F.FeedId=@FeedId)
begin
	CREATE TABLE #tmpProductCategoryMapping (ProductId int, CategoryId int)
	insert into #tmpProductCategoryMapping (ProductId,CategoryId)
	select TOP 1 WITH TIES SM.ProductId,SM.CategoryId
	from Product_Category_Mapping SM WITH (NOLOCK), Category C WITH (NOLOCK)
	where SM.ProductId in (select ProductId from #ExportProductsTmp)
		and SM.CategoryId=C.Id and C.Deleted=0
	ORDER BY ROW_NUMBER() OVER(PARTITION BY SM.ProductId ORDER BY C.Published DESC, SM.DisplayOrder)

	insert  into #tmpFeedDefault (CategoryId, ParentCategoryId, ProductId , Levelsort, FeedCategoryId, CategoryMap, DefaultValuesXML, TypeActiveId)
	select C.Id, C.ParentCategoryId, CM.ProductId,1 as Levelsort,isnull(F.CategoryId,0) as FeedCategoryId,
		F.CategoryMap, F.DefaultValuesXML, isnull(F.TypeActiveId,0) as TypeActiveId
	from Category C WITH (NOLOCK),	
				#tmpProductCategoryMapping CM
					left  join FNS_FeedCategory F WITH (NOLOCK) on CM.CategoryId=F.CategoryId and F.FeedId=@FeedId
	where C.id=CM.CategoryId and C.Deleted=0

	while (@@ROWCOUNT>0)
	begin
		insert  into #tmpFeedDefault (CategoryId, ParentCategoryId, ProductId , Levelsort, FeedCategoryId, CategoryMap, DefaultValuesXML, TypeActiveId)
		select C.Id, C.ParentCategoryId, D.ProductId,D.Levelsort+1 as Levelsort,isnull(F.CategoryId,D.FeedCategoryId) as FeedCategoryId,
			isnull(F.CategoryMap,D.CategoryMap) as CategoryMap, 
			isnull(F.DefaultValuesXML,D.DefaultValuesXML) as DefaultValuesXML, 
			isnull(F.TypeActiveId,D.TypeActiveId) as TypeActiveId
		from Category C WITH (NOLOCK)
			left  join FNS_FeedCategory F WITH (NOLOCK) on C.Id=F.CategoryId and F.FeedId=@FeedId,	
			#tmpFeedDefault D
		where C.id=D.ParentCategoryId and C.Deleted=0
			and C.Id not in (select CategoryId from #tmpFeedDefault)
	end	
	drop table #tmpProductCategoryMapping	
	
	delete from #tmpFeedDefault where FeedCategoryId=0	
end

--FNS_FeedProduct
if exists(select * from FNS_FeedProduct F WITH (NOLOCK) where F.FeedId=@FeedId)
begin
	insert  into #tmpFeedDefault (CategoryId, ParentCategoryId, ProductId , Levelsort, FeedCategoryId, CategoryMap, DefaultValuesXML, TypeActiveId)
	select 0 as CategoryId, 0 as ParentCategoryId, F.ProductId,0 as Levelsort,0 as FeedCategoryId, F.CategoryMap, F.DefaultValuesXML, F.TypeActiveId
	from FNS_FeedProduct F WITH (NOLOCK)
	where F.FeedId=@FeedId
end

--No Export
delete from #ExportProductsTmp where ProductId in (select ProductId from #tmpFeedDefault where TypeActiveId=2)
delete from #tmpFeedDefault where TypeActiveId=2

--paging
DECLARE @PageLowerBound int
DECLARE @PageUpperBound int
DECLARE @RowsToReturn int
SET @RowsToReturn = @PageSize * (@PageIndex + 1)	
SET @PageLowerBound = @PageSize * @PageIndex
SET @PageUpperBound = @PageLowerBound + @PageSize + 1

	CREATE TABLE #PageIndex 
	(
		[IndexId] int IDENTITY (1, 1) NOT NULL,
		[ProductId] int NOT NULL
	)
	INSERT INTO #PageIndex ([ProductId])
	SELECT ProductId
	FROM #ExportProductsTmp
	ORDER BY ProductId
	
	--return products
	create table #ExportProducts (ProductId int Not null) 
	insert into #ExportProducts (ProductId)
	select TOP (@RowsToReturn) [pi].ProductId
	FROM
		#PageIndex [pi]
	WHERE
		[pi].IndexId > @PageLowerBound AND 
		[pi].IndexId < @PageUpperBound
	ORDER BY
		[pi].IndexId
	
	DROP TABLE #PageIndex
	
select P.*
from Product P WITH (NOLOCK)
where P.Id in (select ProductId from #ExportProducts)
ORDER BY P.[Name] ASC

select P.ProductId , P.Levelsort, P.FeedCategoryId, P.CategoryMap, P.DefaultValuesXML,
	A.[ASIN],E.[EPID] 
from #tmpFeedDefault P 
	left join FNS_FeedASIN A WITH (NOLOCK) on P.ProductId=A.ProductId
	left join FNS_FeedEPID E WITH (NOLOCK) on P.ProductId=E.ProductId
where P.ProductId in (select ProductId from #ExportProducts)	
order by P.ProductId , P.Levelsort

drop table #tmpFeedDefault

If (@LanguageId>0)
begin
	select L.EntityId as ProductId,ltrim(rtrim(Substring(L.LocaleValue,1,400))) as Name
	from LocalizedProperty L WITH (NOLOCK) 
	where L.EntityId in (select ProductId from #ExportProducts) and L.LanguageId=@LanguageId and L.LocaleKeyGroup='Product' and L.LocaleKey='Name'

	select L.EntityId as ProductId,L.LocaleValue as ShortDescription
	from LocalizedProperty L WITH (NOLOCK) 
	where L.EntityId in (select ProductId from #ExportProducts) and L.LanguageId=@LanguageId and L.LocaleKeyGroup='Product' and L.LocaleKey='ShortDescription'
			
	select L.EntityId as ProductId,L.LocaleValue as FullDescription
	from LocalizedProperty L WITH (NOLOCK) 
	where L.EntityId in (select ProductId from #ExportProducts) and L.LanguageId=@LanguageId and L.LocaleKeyGroup='Product' and L.LocaleKey='FullDescription'
end
else
begin
	select TOP 0 ProductId,'' as Name
	from #ExportProducts
	
	select TOP 0 ProductId,'' as ShortDescription
	from #ExportProducts

	select TOP 0 ProductId,'' as FullDescription
	from #ExportProducts
end

--UrlRecord
create table #UrlRecord	(ProductId int,SeName nvarchar(400))

insert into #UrlRecord (ProductId,SeName)
select U.EntityId as ProductId,ltrim(rtrim(Substring(U.Slug,1,400))) as SeName
from UrlRecord U WITH (NOLOCK)
where U.EntityId in (select ProductId from #ExportProducts) 
	and U.EntityName='Product' and U.LanguageId=0 and U.IsActive=1	
			
If (@LanguageId>0)
begin
	update #UrlRecord
	set SeName=ltrim(rtrim(Substring(US.Slug,1,400)))
	from #UrlRecord C, (select TOP 1 WITH TIES U.EntityId,U.Slug
		from UrlRecord U WITH (NOLOCK)
		where U.EntityId in (select ProductId from #UrlRecord) 
			and U.EntityName='Product' and U.LanguageId=@LanguageId and U.IsActive=1
			and ltrim(rtrim(U.Slug))!=''
		ORDER BY ROW_NUMBER() OVER(PARTITION BY U.EntityId ORDER BY U.Id DESC)) US
	where C.ProductId=US.EntityId				
end
	
select * from  #UrlRecord
drop table #UrlRecord
--end UrlRecord

--Category
If (@LanguageId>0)
begin
	SELECT TOP 1 WITH TIES PM.ProductId,case 
			when L.LocaleValue IS NULL then C.Name
			when ltrim(rtrim(L.LocaleValue))=''  then C.Name
			else L.LocaleValue
			end as Name
	FROM Product_Category_Mapping PM WITH (NOLOCK),Category C WITH (NOLOCK)
		left join LocalizedProperty L on C.Id=L.EntityId 
					and L.LocaleKeyGroup='Category' 
					and L.LocaleKey='Name' 
					and L.LanguageId=@LanguageId	
	WHERE PM.CategoryId=C.Id and PM.ProductId in (select ProductId from #ExportProducts)
		AND (@StoreId = 0 or C.LimitedToStores = 0 OR EXISTS (
			SELECT 1 FROM [StoreMapping] sm with (NOLOCK)
			WHERE [sm].EntityId = C.Id AND [sm].EntityName = 'Category' and [sm].StoreId=@StoreId))
	ORDER BY ROW_NUMBER() OVER(PARTITION BY PM.ProductId ORDER BY PM.DisplayOrder ASC)
end
else
begin
	SELECT TOP 1 WITH TIES PM.ProductId,C.Name
	FROM Product_Category_Mapping PM WITH (NOLOCK),Category C WITH (NOLOCK)
	WHERE PM.CategoryId=C.Id and PM.ProductId in (select ProductId from #ExportProducts)
		AND (@StoreId = 0 or C.LimitedToStores = 0 OR EXISTS (
			SELECT 1 FROM [StoreMapping] sm with (NOLOCK)
			WHERE [sm].EntityId = C.Id AND [sm].EntityName = 'Category' and [sm].StoreId=@StoreId))
	ORDER BY ROW_NUMBER() OVER(PARTITION BY PM.ProductId ORDER BY PM.DisplayOrder ASC)
end

--end Category

--Manufacturer
If (@LanguageId>0)
begin
	SELECT TOP 1 WITH TIES PM.ProductId,case 
			when L.LocaleValue IS NULL then M.Name
			when ltrim(rtrim(L.LocaleValue))=''  then M.Name
			else L.LocaleValue
			end as Name
	FROM Product_Manufacturer_Mapping PM WITH (NOLOCK),Manufacturer M WITH (NOLOCK)
		left join LocalizedProperty L on M.Id=L.EntityId 
					and L.LocaleKeyGroup='Manufacturer' 
					and L.LocaleKey='Name' 
					and L.LanguageId=@LanguageId	
	WHERE PM.ManufacturerId=M.Id and PM.ProductId in (select ProductId from #ExportProducts)
	ORDER BY ROW_NUMBER() OVER(PARTITION BY PM.ProductId ORDER BY PM.DisplayOrder ASC)
end
else
begin
	SELECT TOP 1 WITH TIES PM.ProductId,M.Name
	FROM Product_Manufacturer_Mapping PM WITH (NOLOCK),Manufacturer M WITH (NOLOCK)
	WHERE PM.ManufacturerId=M.Id and PM.ProductId in (select ProductId from #ExportProducts)
	ORDER BY ROW_NUMBER() OVER(PARTITION BY PM.ProductId ORDER BY PM.DisplayOrder ASC)
end
--end Manufacturer

--Picture
	SELECT PM.ProductId,PM.DisplayOrder,P.Id,P.IsNew,P.MimeType,P.SeoFilename
	FROM Product_Picture_Mapping PM WITH (NOLOCK),Picture P WITH (NOLOCK)
	WHERE PM.PictureId=P.Id and PM.ProductId in (select ProductId from #ExportProducts)
	ORDER BY PM.ProductId,PM.DisplayOrder ASC
--end Picture

--Product Specification
If (@LanguageId>0)
begin
	select S.Id,PM.ProductId,
		case 
			when PM.CustomValue is not null and ltrim(rtrim(PM.CustomValue))!='' then PM.CustomValue
			when LSO.LocaleValue IS NULL or ltrim(rtrim(LSO.LocaleValue))='' then SO.Name
			else LSO.LocaleValue
			end as Value
	from SpecificationAttribute S WITH (NOLOCK),
		SpecificationAttributeOption SO WITH(NOLOCK)
			left join LocalizedProperty LSO on SO.Id=LSO.EntityId 
					and LSO.LocaleKeyGroup='SpecificationAttributeOption' 
					and LSO.LocaleKey='Name' 
					and LSO.LanguageId=@LanguageId, 
		Product_SpecificationAttribute_Mapping PM WITH(NOLOCK)
	where S.Id=SO.SpecificationAttributeId 
		and SO.Id=PM.SpecificationAttributeOptionId
		and PM.ShowOnProductPage=1
		--and PM.AttributeTypeId=0 --3.50
		and PM.ProductId in (select ProductId from #ExportProducts)
	order by PM.ProductId,S.Name
end
else
begin
	select S.Id,PM.ProductId,
		case 
			when PM.CustomValue is not null and ltrim(rtrim(PM.CustomValue))!='' then PM.CustomValue
			else SO.Name
		end as Value
	from SpecificationAttribute S WITH (NOLOCK), 
		SpecificationAttributeOption SO WITH(NOLOCK), 
		Product_SpecificationAttribute_Mapping PM WITH(NOLOCK)
	where S.Id=SO.SpecificationAttributeId 
		and SO.Id=PM.SpecificationAttributeOptionId
		and PM.ShowOnProductPage=1
		--and PM.AttributeTypeId=0 --3.50
		and PM.ProductId in (select ProductId from #ExportProducts)
	order by PM.ProductId,S.Name
end
--end Product Specification

drop table #ExportProducts
END
GO
IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[Tri_Insert_Product_FeedManager]'))
DROP TRIGGER [dbo].[Tri_Insert_Product_FeedManager]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[Tri_Insert_Product_FeedManager] ON [dbo].[Product]
FOR INSERT
AS
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FNS_FeedProduct]') AND type in (N'U'))
		and EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FNS_FeedRec]') AND type in (N'U'))
begin
	insert into FNS_FeedProduct (FeedId,ProductId,CategoryMap,DefaultValuesXML,TypeActiveId)
	select F.Id,I.Id,null,null,2
	from FNS_FeedRec F WITH (NOLOCK),inserted I
	where F.DoNotExportNewProducts=1
end		
GO
EXEC sp_settriggerorder @triggername=N'[dbo].[Tri_Insert_Product_FeedManager]', @order=N'Last', @stmttype=N'INSERT'
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[FNS_FeedManager_CopyFromFroogle]') AND type in (N'P', N'PC'))
DROP PROCEDURE [FNS_FeedManager_CopyFromFroogle]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FNS_FeedManager_CopyFromFroogle]
(
	@FeedId		int
)
AS
BEGIN
SET NOCOUNT ON
	insert into FNS_FeedProduct (FeedId,ProductId,CategoryMap,DefaultValuesXML,TypeActiveId)
	select @feedid,G.ProductId,G.Taxonomy,
		'<DefaultValues>'+
			+case
			when ltrim(rtrim(G.Gender))!='' then '<DefaultValue><FeedAttributeId>17</FeedAttributeId><Value>'+ltrim(rtrim(G.Gender))+'</Value></DefaultValue>'
			else ''
			end
			+case
			when ltrim(rtrim(G.AgeGroup))!='' then '<DefaultValue><FeedAttributeId>18</FeedAttributeId><Value>'+ltrim(rtrim(G.AgeGroup))+'</Value></DefaultValue>'
			else ''
			end
			+case
			when ltrim(rtrim(G.Color))!='' then '<DefaultValue><FeedAttributeId>19</FeedAttributeId><Value>'+ltrim(rtrim(G.Color))+'</Value></DefaultValue>'
			else ''
			end
			+case
			when ltrim(rtrim(G.Size))!='' then '<DefaultValue><FeedAttributeId>20</FeedAttributeId><Value>'+ltrim(rtrim(G.Size))+'</Value></DefaultValue>'
			else ''
			end
		+'</DefaultValues>' as DefaultValuesXML,0 as TypeActiveId
	from GoogleProduct G
	where (ltrim(rtrim(G.Gender))!=''
		or ltrim(rtrim(G.AgeGroup))!=''
		or ltrim(rtrim(G.Color))!=''
		or ltrim(rtrim(G.Size))!='')
		and G.ProductId not in (select ProductId from FNS_FeedProduct where FeedId=@feedid)
end
GO
IF not EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FNS_FeedASIN]') AND name = N'IX_FNS_FeedASIN_ASIN')
	CREATE NONCLUSTERED INDEX IX_FNS_FeedASIN_ASIN ON FNS_FeedASIN ([ASIN] DESC) 
GO
IF not EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[FNS_FeedEPID]') AND name = N'IX_FNS_FeedEPID_EPID')
	CREATE NONCLUSTERED INDEX IX_FNS_FeedEPID_EPID ON FNS_FeedEPID ([EPID] DESC) 
GO
