USE [ExcambiiApplication]
GO
/****** Object:  StoredProcedure [swapnal].[SP_UpdateProductLikes]    Script Date: 5/18/2015 3:36:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Code King Maker
-- Create date: 16/05/2015
-- Description:	Like/Unlike product 
-- =============================================
ALTER PROCEDURE [swapnal].[SP_UpdateProductLikes]
	-- Add the parameters for the stored procedure here
	@UserCode AS [uniqueidentifier] = NULL,
	@ProductCode AS [uniqueidentifier] = NULL,
	@IsLiked AS BIT = 0
AS
BEGIN
	SET NOCOUNT ON;
	
	UPDATE	SellerProductCatalogue
	SET		LikeCount =  CASE WHEN @IsLiked = 1 THEN ISNULL(LikeCount,0) + 1 ELSE case when ISNULL(LikeCount,0) != 0 then  ISNULL(LikeCount,0) - 1 ELSE 0 END END			
	WHERE	productid = @ProductCode

	IF(@IsLiked = 1)
	BEGIN
		INSERT INTO tblUserLikedProducts
		(
			UserCode,
			ProductCode,
			LikedDate
		)
		VALUES
		(
			@UserCode,
			@ProductCode,
			GETDATE()
		)
	END
	ELSE
	BEGIN
		DELETE
		FROM	tblUserLikedProducts
		WHERE	UserCode = @UserCode
				AND ProductCode = @ProductCode
	END

	SELECT	1 AS ResultValue,
			'Success' AS Result, 
			LikeCount AS LikeCount
	FROM	SellerProductCatalogue
	WHERE	productid = @ProductCode
END
