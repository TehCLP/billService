using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for alert
/// </summary>
public static class alert
{
	public static string Error(string strAlert)
	{
        return Alert(strAlert, 1);
	}
    public static string Warning(string strAlert)
    {
        return Alert(strAlert, 2);
    }
    public static string Infor(string strAlert)
    {
        return Alert(strAlert, 3);
    }
    public static string Confirm(string strAlert)
    {
        return Alert(strAlert, 4);
    }

    private static string Alert(string strAlert, int type)
    {
        // type = 1 => error
        // type = 2 => warning
        // type = 3 => information
        // type = 4 => confirmation

        string typeName = "";
        switch (type)
        {
            case 1: typeName = "error-box"; break;
            case 2: typeName = "warning-box"; break;
            case 3: typeName = "information-box"; break;
            case 4: typeName = "confirmation-box"; break;
            default: typeName = "error-box"; break;
        }

        string strTmp = string.Format(@"<div class='{0} round'>{1}</div>", typeName, strAlert);

        return strTmp;
    }
}
