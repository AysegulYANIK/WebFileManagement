using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//CONFIGURATION REQUIREMENTS #1
using System.Data.SqlClient; //this namespace is for sqlclient server  
using System.Configuration; // this namespace is add I am adding connection name in web config file config connection name  

public partial class LoginPage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["mycon"].ToString());  
   
        try{  
                string uid = TextBox1.Text;  
                string pass = TextBox2.Text;  
                con.Open();  
                string qry = "select * from Ulogin where UserId='" + uid + "' and Password='" + pass + "'";  
                SqlCommand cmd = new SqlCommand(qry,con);  
                SqlDataReader sdr = cmd.ExecuteReader();  
                if(sdr.Read())  
                {  
                    Label4.Text = "Login Sucess";
                    Response.Redirect("Upload-Download.aspx");
                }  
                else  
                {  
                    Label4.Text = "UserId and Password pair does not match, try again please.";  
                }  
                con.Close();  
        }  
        catch(Exception ex)  
                {  
                    Response.Write(ex.Message);  
                }  
           
  
    }
}
