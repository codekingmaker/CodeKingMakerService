
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Codekingmaker
-- Create date: 17th May 2015
-- Description:	Get all products
-- =============================================
ALTER PROCEDURE SP_GetAllProducts
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	p.productid as [ProductCode],
			p.productcode as [code],
			p.name_title as [Title],
			p.categoryid as [GroupCode],
			' ' as [SubGroupCode],
			' ' as [Origin],
		    ' ' as [Material],
		    ' ' as [Condition],
		    p.[Description],
		    p.rate_mrp as [PriceRetail],
		    p.rate_min_sell as [PriceSelling],
		    p.sellerid as [BusinessCode],
		    p.[createdby],
			p.[CreatedOn],    
		    p.[UpdatedBy],    
		    p.[UpdatedOn],    
		    (SELECT top 1 u.imageurl from dbo.SellerProductImageURLCatalogue u (nolock) where u.saleid = p.saleid) as imageurl,  
		    s.name,  
		    b.id,  
		    ISNULL(b.firstname,'') + ' ' + ISNULL(b.middlename, '') + ' ' + ISNULL(b.lastname,'') as FullName,
			LikeCount
   FROM	SellerProductCatalogue p (nolock)  
   INNER JOIN SaleTypeCatalogue s (nolock) on p.saletypeid = s.id  
   INNER JOIN BusinessCatalogue b (nolock) on p.sellerid = b.id
   ORDER BY p.[UpdatedOn] DESC  
END
GO
