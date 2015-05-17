using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using ExcombiiAPI.BusinessLayer;

namespace ExcombiiAPI
{
    /// <summary>
    /// Summary description for ExcombiiService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ExcombiiService : System.Web.Services.WebService
    {

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void RegisterNewUser(string firstName, string lastName, string password, DateTime dateOfBirth, string emailAddress, string mobileNo, string sSocialNetworkID)
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            BLUser objUser = new BLUser();
            objUser.FirstName = firstName;
            objUser.LastName = lastName;
            objUser.DateOfBirth = dateOfBirth;
            objUser.Email = emailAddress;
            objUser.Password = password;
            objUser.MobileNo = mobileNo;
            objUser.sSocialNetworkID = sSocialNetworkID;
            objUser = objUser.RegisterNewUser();

            //Hashtable objResult = new Hashtable();
            //objResult.Add("ResultValue", objUser.IResultNo);
            //objResult.Add("Result", objUser.SResult);

            Context.Response.Write(oSerializer.Serialize(objUser));
            Context.Response.Flush();
            Context.Response.Clear();
            Context.Response.End();
        }

        #region Login Validation
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void Login(string emailAddress, string password)
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLUser objUser = new BLUser();
            objUser.Email = emailAddress;
            objUser.Password = password;
            objUser = objUser.Login();

            Hashtable objResult = new Hashtable();
            objResult.Add("ResultValue", objUser.IResultNo);
            objResult.Add("Result", objUser.SResult);

            if (objUser.IResultNo == 1)
            {
                objResult.Add("FirstName", objUser.FirstName);
                objResult.Add("LastName", objUser.LastName);
                objResult.Add("UserCode", objUser.SUserCode);
            }

            Context.Response.Write(oSerializer.Serialize(objResult));
            Context.Response.Flush();
            Context.Response.Clear();
            Context.Response.End();
        }
        #endregion

        #region Product Like/Unlike
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void UpdateProductLike(string sUserCode, string sProductId, bool IsLiked)
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLProduct objProduct = new BLProduct();
            objProduct.sProductCode = sProductId;
            objProduct = objProduct.UpdateProductLike(sUserCode, IsLiked);

            Hashtable objResult = new Hashtable();
            objResult.Add("ResultValue", objProduct.IResultNo);
            objResult.Add("Result", objProduct.SResult);

            if (objProduct.IResultNo == 1)
            {
                objResult.Add("LikeCount", objProduct.iProductLikeCount);
                objResult.Add("ProductCode", objProduct.sProductCode);
                objResult.Add("UserCode", sUserCode);
            }

            Context.Response.Write(oSerializer.Serialize(objResult));
            Context.Response.Flush();
            Context.Response.Clear();
            Context.Response.End();
        }
        #endregion

        #region Get all products
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public void GetAllProducts()
        {
            System.Web.Script.Serialization.JavaScriptSerializer oSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            BLProduct objProduct = new BLProduct();
            List<BLProduct> objLstProducts = objProduct.GetAllProducts();

            Context.Response.Write(oSerializer.Serialize(objLstProducts));
            Context.Response.Flush();
            Context.Response.Clear();
            Context.Response.End();
        }
        #endregion
    }
}
