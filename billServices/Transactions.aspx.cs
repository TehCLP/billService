using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Transactions : System.Web.UI.Page
{
    Access_Conn conn = new Access_Conn();
    private const string _ssid = "QryGVListCustomter";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bindDDL();

            Session.Remove(_ssid);
            Session[_ssid] = "select p.PID,ProjectName,CID,CName,[No] from tbProjects p inner join tbCustomers c on p.PID = c.PID where p.PID = {0}";

            clear();
        }
        try { bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject); }
        catch (Exception ex) { string sssss = ex.ToString(); Response.Redirect(""); }
    }

    private void bindDDL()
    {
        string sql = "select PID,ProjectName from tbProjects";
        string txt = "ProjectName";
        string val = "PID";
        conn.getDataToDDL(ref ddlProjectName, sql, txt, val);
    }
    private void bindData(string sql, GridView gv)
    {
        //System.Data.DataTable dt = new System.Data.DataTable();
        //dt = conn.getDataTable(sql);

        //gv.DataSource = dt;
        //gv.DataBind();
    }

    protected void ddlProjectName_Changed(object sender, EventArgs e)
    {
        string sql = string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue);
        bindData(sql, gvListProject);
        clear();
    }
    protected void btnSave_click(object sender, EventArgs e)
    {
        /*
        if (chkTB(txtNo) && chkTB(txtCustName))
        {
            try
            {
                string sCID = btnSave.CommandArgument;
                string sCol, sVal;
                if (string.IsNullOrEmpty(sCID))
                {
                    // Insert
                    sCol = "CID,PID,CName,[No],CreateDate";
                    sVal = string.Format("{0}|{1}|{2}|{3}|{4}"
                                    , conn.autoIdentity("tbCustomers", "CID")
                                    , ddlProjectName.SelectedValue
                                    , txtCustName.Text.Trim()
                                    , txtNo.Text.Trim()
                                    , "^NOW()");
                    conn.getInsertToDB("tbCustomers", sCol, sVal);
                    ltAlert.Text = alert.Infor("บันทึก เรียบร้อย");
                }
                else
                {
                    // Update
                    sCol = "CID,PID,CName,[No]";
                    sVal = string.Format("{0}|{1}|{2}|{3}"
                                    , sCID
                                    , ddlProjectName.SelectedValue
                                    , txtCustName.Text.Trim()
                                    , txtNo.Text.Trim());
                    conn.getUpdateToDB("tbCustomers", sCol, sVal);
                    ltAlert.Text = alert.Infor("แก้ไข เรียบร้อย");
                }
                clear();
                bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject);
            }
            catch (Exception ex)
            {
                ltAlert.Text = alert.Error(ex.ToString());
            }
        }
        else
        {
            ltAlert.Text = alert.Warning("กรุณากรอกข้อมูลให้ครบ");
        }
        */
    }
    protected void btnCancel_click(object sender, EventArgs e)
    {
        clear();
    }

    protected void gvListProject_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName.ToString())
        {
            case "bEdit":
                showEdit(e.CommandArgument.ToString().Trim());
                break;

            default: break;
        }
    }
    protected void gvListProject_DataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LinkButton btnNo = (LinkButton)e.Row.FindControl("btnNo");
            if (btnNo != null)
            {
                btnNo.Text = DataBinder.Eval(e.Row.DataItem, "No").ToString();
                btnNo.CommandArgument = DataBinder.Eval(e.Row.DataItem, "CID").ToString().Trim();
            }
        }
    }
    protected void gvListProject_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvListProject.PageIndex = e.NewPageIndex;
        bindData(string.Format(Session[_ssid].ToString(), ddlProjectName.SelectedValue), gvListProject);
    }

    private void showEdit(string cID)
    {
        string sql = string.Format(Session[_ssid].ToString(), cID);
        System.Data.DataTable dt = new System.Data.DataTable();
        dt = conn.getDataTable(sql);
        if (dt != null && dt.Rows.Count > 0)
        {
            /*
            txtNo.Text = dt.Rows[0]["No"].ToString();
            txtCustName.Text = dt.Rows[0]["CName"].ToString();

            btnSave.CommandArgument = cID;
            */ 
        }
        ltAlert.Text = string.Empty;
        dt.Dispose();
        conn.DisConnect();
    }

    private void clear()
    {
        conn.clearValue(pnlMasterForm, "ddlProjectName");
        ltAlert.Text = string.Empty;
        //btnSave.CommandArgument = string.Empty;
    }
    private bool chkTB(TextBox txt)
    {
        return (txt.Text.Trim() != string.Empty);
    }
    private bool isNum(string inputTxt)
    {
        System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("^([1-9]|[1-9][0-9]+)$");
        return re.IsMatch(inputTxt);
    }
    
}
