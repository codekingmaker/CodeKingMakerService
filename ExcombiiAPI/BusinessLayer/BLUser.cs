using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;
using DataLayer;

namespace ExcombiiAPI.BusinessLayer
{
    public class BLUser : BLCommon
    {
        #region string properties
        private string _firstName = "";

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; }
        }

        private string _lastName = "";

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
        public string Email { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public DateTime DateOfBirth { get; set; }

        private string _sUserCode = "";

        public string SUserCode
        {
            get { return _sUserCode; }
            set { _sUserCode = value; }
        }

        public string sSocialNetworkID { get; set; }
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

        #region Registration page methods
        public BLUser RegisterNewUser()
        {
            DbDataReader DbReader = null;
            try
            {
                InitializeDb();
                List<DbParams> objLstDbParams = new List<DbParams>();
                objLstDbParams.Add(new DbParams(DbType.String, 100, _firstName, "@FirstName", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.String, 100, _lastName, "@LastName", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.String, 100, Password, "@Password", ParameterDirection.Input));

                if (DateOfBirth.ToString("dd/MM/yyyy") != "01/01/1900")
                    objLstDbParams.Add(new DbParams(DbType.DateTime, 20, DateOfBirth, "@DateOfBirth", ParameterDirection.Input));

                objLstDbParams.Add(new DbParams(DbType.String, 50, Email, "@EmailAddress", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.String, 20, MobileNo, "@MobileNo", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.String, 500, sSocialNetworkID, "@SocialNetworkID", ParameterDirection.Input));

                DbReader = ObjDbfactory.GetReader("SP_CreateNewuser", false, objLstDbParams);

                while (DbReader.Read())
                {
                    IResultNo = ToInteger(DbReader["ResultValue"]);
                    SResult = ToString(DbReader["Result"]);

                    if (IResultNo == 1)
                    {
                        _firstName = ToString(DbReader["FirstName"]);
                        _lastName = ToString(DbReader["LastName"]);
                        DateOfBirth = ToDateTime(DbReader["DateOfBirth"]);
                        Email = ToString(DbReader["Email"]);
                        MobileNo = ToString(DbReader["ZipCode"]);
                    }
                }

                DbReader.Close();
                ObjDbfactory.CloseConnection();
            }
            catch (Exception ex)
            {
                SendLogMail("BLUser : RegisterNewUser", ex.Message);
            }

            return this;
        }
        #endregion

        public BLUser Login()
        {
            DbDataReader dbReader = null;
            try
            {
                InitializeDb();
                List<DbParams> objLstDbParams = new List<DbParams>();
                objLstDbParams.Add(new DbParams(DbType.String, 100, Password, "@Password", ParameterDirection.Input));
                objLstDbParams.Add(new DbParams(DbType.String, 50, Email, "@EmailAddress", ParameterDirection.Input));
                dbReader = ObjDbfactory.GetReader("SP_Validateuser", false, objLstDbParams);

                while (dbReader.Read())
                {
                    _sUserCode = ToString(dbReader["UserCode"]);
                    _firstName = ToString(dbReader["FirstName"]);
                    _lastName = ToString(dbReader["LastName"]);
                    IResultNo = 1;
                    SResult = "Success";
                }

                if (IResultNo == 0)
                    SResult = "Invalid username and password";

                dbReader.Close();
                ObjDbfactory.CloseConnection();
            }
            catch (Exception ex)
            {
                SendLogMail("BLUser : Login", ex.Message);
            }

            return this;
        }
    }
}