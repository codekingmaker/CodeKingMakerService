﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using DataLayer;

namespace ExcombiiAPI.BusinessLayer
{
    public class BLProduct : BLCommon
    {
        #region Product Properties
        private string _sSelleerID = "";

        public string SSelleerID
        {
            get { return _sSelleerID; }
            set { _sSelleerID = value; }
        }
        public string sProductCode { get; set; }
        public int iProductLikeCount { get; set; }

        private string sCode = "";

        public string SCode
        {
            get { return sCode; }
            set { sCode = value; }
        }

        private string _sTitle = "";

        public string STitle
        {
            get { return _sTitle; }
            set { _sTitle = value; }
        }

        private string _sSubGroupCode = "";

        public string SSubGroupCode
        {
            get { return _sSubGroupCode; }
            set { _sSubGroupCode = value; }
        }

        private string _sOrigin = "";

        public string SOrigin
        {
            get { return _sOrigin; }
            set { _sOrigin = value; }
        }

        private string _sMaterial = "";

        public string SMaterial
        {
            get { return _sMaterial; }
            set { _sMaterial = value; }
        }

        private string _sCondition = "";

        public string SCondition
        {
            get { return _sCondition; }
            set { _sCondition = value; }
        }

        private string _sDescription = "";

        public string SDescription
        {
            get { return _sDescription; }
            set { _sDescription = value; }
        }

        private string _sCreatedBy = "";

        public string SCreatedBy
        {
            get { return _sCreatedBy; }
            set { _sCreatedBy = value; }
        }

        private string _sUpdatedBy = "";

        public string SUpdatedBy
        {
            get { return _sUpdatedBy; }
            set { _sUpdatedBy = value; }
        }

        private string _sImageUrl = "";

        public string SImageUrl
        {
            get { return _sImageUrl; }
            set { _sImageUrl = value; }
        }

        private string _sProductName = "";

        public string SProductName
        {
            get { return _sProductName; }
            set { _sProductName = value; }
        }

        private string _sBusinessid = "";

        public string SBusinessid
        {
            get { return _sBusinessid; }
            set { _sBusinessid = value; }
        }

        private string _sFullName = "";

        public string SFullName
        {
            get { return _sFullName; }
            set { _sFullName = value; }
        }
        #endregion

        #region integer properties
        private int _iCategoryId = 0;

        public int ICategoryId
        {
            get { return _iCategoryId; }
            set { _iCategoryId = value; }
        }
        #endregion

        #region double properties
        private double _dblPriceRetail = 0D;

        public double DblPriceRetail
        {
            get { return _dblPriceRetail; }
            set { _dblPriceRetail = value; }
        }

        private double _dblMinPrice = 0D;

        public double DblMinPrice
        {
            get { return _dblMinPrice; }
            set { _dblMinPrice = value; }
        }
        #endregion

        #region datetim properties
        private DateTime? _dtCreatedOn;

        public DateTime? DtCreatedOn
        {
            get { return _dtCreatedOn; }
            set { _dtCreatedOn = value; }
        }

        private DateTime? dtUpdatedOn;

        public DateTime? DtUpdatedOn
        {
            get { return dtUpdatedOn; }
            set { dtUpdatedOn = value; }
        }
        #endregion

        #region Object related variables
        DbFactory ObjDbfactory;
        #endregion

        #region Initialize database connection method
        protected void InitializeDb()
        {
            ObjDbfactory = new DbFactory(DataBaseType.SQLServer, ConfigurationManager.ConnectionStrings["ExCambiiConnectionString"].ConnectionString);
        }
        #endregion

        /// <summary>
        /// Like/Unlike product by an user
        /// </summary>
        /// <param name="sUserCode">Unique id of the user</param>
        /// <param name="IsLiked">is user like the product or not</param>
        /// <returns></returns>
        public BLProduct UpdateProductLike(string sUserCode, bool IsLiked)
        {
            DbDataReader dbReader = null;
            try
            {
                InitializeDb();
                List<DbParams> objLstDbParams = new List<DbParams>();
                objLstDbParams.Add(new DbParams(DbType.Guid, 100, sUserCode, "@UserCode", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.Guid, 100, sProductCode, "@ProductCode", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.Boolean, 100, IsLiked, "@IsLiked", ParameterDirection.Input));
                dbReader = ObjDbfactory.GetReader("SP_UpdateProductLikes", false, objLstDbParams);

                while (dbReader.Read())
                {
                    IResultNo = ToInteger(dbReader["ResultValue"]);
                    SResult = ToString(dbReader["Result"]);

                    if (IResultNo == 1)
                        iProductLikeCount = ToInteger(dbReader["LikeCount"]);
                }

                dbReader.Close();
                ObjDbfactory.CloseConnection();
            }
            catch (Exception ex)
            {
                SendLogMail("BLProduct : LikeProduct", ex.Message);

                if (ObjDbfactory != null)
                    ObjDbfactory.CloseConnection();
            }
            return this;
        }

        public List<BLProduct> GetAllProducts()
        {
            List<BLProduct> objLstProducts = new List<BLProduct>();
            DbDataReader dbReader = null;
            try
            {
                InitializeDb();
                List<DbParams> objLstDbParams = new List<DbParams>();
                dbReader = ObjDbfactory.GetReader("SP_GetAllProducts", false, objLstDbParams);

                while (dbReader.Read())
                {
                    BLProduct objProduct = new BLProduct();
                    objProduct.sProductCode = ToString(dbReader["ProductCode"]);
                    objProduct.sCode = ToString(dbReader["code"]);
                    objProduct.STitle = ToString(dbReader["Title"]);
                    objProduct.ICategoryId = ToInteger(dbReader["GroupCode"]);
                    objProduct.SSubGroupCode = ToString(dbReader["SubGroupCode"]);
                    objProduct.SOrigin = ToString(dbReader["Origin"]);
                    objProduct.SMaterial = ToString(dbReader["Material"]);
                    objProduct.SCondition = ToString(dbReader["Condition"]);
                    objProduct.SDescription = ToString(dbReader["Description"]);
                    objProduct.DblPriceRetail = ToDouble(dbReader["PriceRetail"]);
                    objProduct.DblMinPrice = ToDouble(dbReader["PriceSelling"]);
                    objProduct.SSelleerID = ToString(dbReader["BusinessCode"]);
                    objProduct.SCreatedBy = ToString(dbReader["createdby"]);
                    objProduct.DtCreatedOn = ToDateTime(dbReader["CreatedOn"]);
                    objProduct.SUpdatedBy = ToString(dbReader["UpdatedBy"]);
                    objProduct.dtUpdatedOn = ToDateTime(dbReader["UpdatedOn"]);
                    objProduct.SImageUrl = ToString(dbReader["imageurl"]);
                    objProduct.SProductName = ToString(dbReader["name"]);
                    objProduct.SBusinessid = ToString(dbReader["id"]);
                    objProduct.SFullName = ToString(dbReader["FullName"]);
                    objProduct.iProductLikeCount = ToInteger(dbReader["LikeCount"]);

                    objLstProducts.Add(objProduct);
                }

                dbReader.Close();
                ObjDbfactory.CloseConnection();
            }
            catch (Exception ex)
            {
                SendLogMail("BLProduct : GetAllProducts", ex.Message);

                if (ObjDbfactory != null)
                    ObjDbfactory.CloseConnection();
            }
            return objLstProducts;
        }

    }
}