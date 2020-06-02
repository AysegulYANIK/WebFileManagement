using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Security.Claims;
using System.Security;

public partial class Upload_Download : System.Web.UI.Page
{

    private string GetFileTypeByExtension(string extension)
    {
        switch (extension.ToLower())
        {
            case ".doc":

            case ".docx":
                return "Microsoft Word Document";

            case ".xlsx":
            case "xls":
                return "Microsoft Excel Document";

            case ".txt":
                return "Text Document";

            case ".png":
            case ".jpg":
                return "Image";
            default:
                return "Unknown";
        }
    }

    public void Create_Table()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("File", typeof(string));
        dt.Columns.Add("Size", typeof(string));
        dt.Columns.Add("Type", typeof(string));

        //DO FOR EACH FILE IN THE DIRECTORY
        foreach (string strFile in Directory.GetFiles(Server.MapPath("/files/")))
        {
            FileInfo fi = new FileInfo(strFile);
            dt.Rows.Add(fi.Name, fi.Length, GetFileTypeByExtension(fi.Extension));
        }

        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Label1.Text = "";
        Label2.Text = "";
        Create_Table();

        if (User.Identity.IsAuthenticated)
        {
            Page.Title = "Home page for " + User.Identity.Name;
        }
        else
        {
            Page.Title = "Home page for Guest User";
        }
    }

    //FILE UPLOAD
    protected void Button1_Click(object sender, EventArgs e)
    {

        //IF A FILE CHOOSEN
        if (FileUpload1.HasFile)
        {
            Label1.Text = "The file which you've choosen will be started to upload in a few seconds. Please wait.";

            int length = FileUpload1.PostedFile.ContentLength;


            //FIND SERVER PATH TO DECIDE WHERE TO SAVE
            string path = Server.MapPath("/files/");

            //IF FILE IS SMALL ENOUGH
            if (length <= 2097152)
            {
                try
                {
                    FileUpload1.PostedFile.SaveAs(path + FileUpload1.FileName);
                    Label2.Text = "The file 's been uploaded.";
                }

                catch
                {
                    Label2.Text = "Uploading process corrupted. Sorry.";
                }
            }
            else
            {
                Label1.Text = "This file is greater than 2MB which is the limit of filesize.";
            }

        }
        else
        {
            Label1.Text = "You did not choose any file."; 
        }
        Create_Table();
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Download")
        {
            Response.Clear();
            Response.ContentType = "application-octect-stream";
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + e.CommandArgument);
            Response.TransmitFile(Server.MapPath("/files/") + e.CommandArgument);
            Response.End();
        }
    }

}
