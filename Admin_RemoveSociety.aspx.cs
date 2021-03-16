using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SocietyManagment
{
  public partial class Admin_RemoveSociety : System.Web.UI.Page
  {
    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString.ToString());
    protected void Page_Load(object sender, EventArgs e)
    {
      lblAdminName.Text = Session["AName"].ToString();
      if (!IsPostBack)
      {
        con.Open();
        String s = "select SocietyCode from SocietyInfo";
        SqlCommand cmd = new SqlCommand(s, con);
        SqlDataReader dr = cmd.ExecuteReader();
       
        dlRScode.DataSource = dr;
        dlRScode.DataTextField = "SocietyCode";
        dlRScode.DataValueField = "SocietyCode";
        dlRScode.DataBind();
        dlRScode.Items.Insert(0, "Select Society Code");
      }
    }
    protected void ALogout_Click(Object sender, EventArgs e)
    {
      Session.Abandon();
      Response.Redirect("AdminLogin.aspx");
    }

    protected void btnRSRemove_Click(object sender, EventArgs e)
    {
      if (rbRSNo.Checked == true)
      {
        lblRSflag.Text = "Please give the confirmation first".ToString();
      }
      else
      {
        String check_pass = "";
        con.Open();
        String q = "select APassword from AdminInfo where AName=@p1";
        SqlCommand cmd = new SqlCommand(q, con);
        cmd.Parameters.AddWithValue("@p1", Session["AName"].ToString());
        cmd.ExecuteNonQuery();
        SqlDataReader rdr = cmd.ExecuteReader();
        while (rdr.Read())
        {
           check_pass = rdr["APassword"].ToString();
        }
        con.Close();
        if (check_pass == txtRSAdminPass.Text)
        {
          con.Open();
          // delete society
          string s = "delete from SocietyInfo where SocietyCode=@p2";
          SqlCommand cmd1 = new SqlCommand(s, con);
          cmd1.Parameters.AddWithValue("@p2", dlRScode.SelectedItem.Value.ToString());
          cmd1.ExecuteNonQuery();
          //delete Committee member
          string s1 = "delete from CommitteeInfo where SocietyCode=@p3";
          SqlCommand cmd2 = new SqlCommand(s1, con);
          cmd2.Parameters.AddWithValue("@p3", dlRScode.SelectedItem.Value.ToString());
          cmd2.ExecuteNonQuery();

          // delete members
           string s2 = "delete from MemberInfo where SocietyCode=@p4";
           SqlCommand cmd3 = new SqlCommand(s2, con);
           cmd3.Parameters.AddWithValue("@p4", dlRScode.SelectedItem.Value.ToString());
           cmd3.ExecuteNonQuery();
          lblRSflag.Text = "Deleted".ToString();
          dlRScode.SelectedIndex = 0;
          txtRSAdminPass.Text = "";
          txtRSName.Text = "";
        }
        else
        {
          lblRSflag.Text = "Password is incorrect".ToString();
        }
      }

    }
    protected void Clear()
    {
      txtRSAdminPass.Text = "";
      txtRSName.Text = "";
    }

    protected void dlRScode_SelectedIndexChanged(object sender, EventArgs e)
    {
      if (dlRScode.SelectedIndex != 0)
      {
        con.Open();
        String fetchdata = "select * from SocietyInfo where SocietyCode='"+dlRScode.SelectedValue.ToString()+"'";
        SqlCommand cmd4 = new SqlCommand(fetchdata, con);
        SqlDataReader dr = cmd4.ExecuteReader();
        while (dr.Read())
        {
          txtRSName.Text = dr["SocietyName"].ToString();
        }
      }
      else
      {
        Clear();
      }
    }
  }
}
