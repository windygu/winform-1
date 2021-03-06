﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web;
using JCodes.Framework.Common;
using JCodes.Framework.Data.Model;

namespace JCodes.Framework.WebUI.admin.ashx
{
    /// <summary>
    /// 按钮表操作
    /// </summary>
    public class bg_button : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            string action = context.Request.Params["action"];
            UserOperateLog userOperateLog = null;   //操作日志对象
            try
            {
                Data.Model.User user = UserHelper.GetUser(context);   //获取cookie里的用户对象
                userOperateLog = new UserOperateLog();
                userOperateLog.UserIp = context.Request.UserHostAddress;
                userOperateLog.UserName = user.UserId;

                switch (action)
                {
                    case "getbutton":   //根据用户的权限获取用户点击的菜单有权限的按钮
                        string pageName = context.Request.Params["pagename"];
                        string menuCode = context.Request.Params["menucode"];   //菜单标识码
                        DataTable dt = new Data.BLL.Button().GetButtonByMenuCodeAndUserId(menuCode, user.Id);
                        context.Response.Write(ToolbarHelper.GetToolBar(dt, pageName));
                        break;
                    case "search":
                        string strWhere = "1=1";
                        string sort = context.Request.Params["sort"];  //排序列
                        string order = context.Request.Params["order"];  //排序方式 asc或者desc
                        int pageindex = int.Parse(context.Request.Params["page"]);
                        int pagesize = int.Parse(context.Request.Params["rows"]);

                        int totalCount;   //输出参数
                        string strJson = new Data.BLL.Button().GetPager("tbButton", "Id,Name,Code,Icon,Sort,AddDate,Description", sort + " " + order, pagesize, pageindex, strWhere, out totalCount);
                        context.Response.Write("{\"total\": " + totalCount.ToString() + ",\"rows\":" + strJson + "}");
                        userOperateLog.OperateInfo = "查询按钮";
                        userOperateLog.IfSuccess = true;
                        userOperateLog.Description = "查询条件：" + strWhere + " 排序：" + sort + " " + order + " 页码/每页大小：" + pageindex + " " + pagesize;
                        Data.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
                        break;
                    default:
                        context.Response.Write("{\"result\":\"参数错误！\",\"success\":false}");
                        break;
                }
            }
            catch (Exception ex)
            {
                context.Response.Write("{\"msg\":\"" + JsonHelper.StringFilter(ex.Message) + "\",\"success\":false}");
                userOperateLog.OperateInfo = "按钮功能异常";
                userOperateLog.IfSuccess = false;
                userOperateLog.Description = JsonHelper.StringFilter(ex.Message);
                Data.BLL.UserOperateLog.InsertOperateInfo(userOperateLog);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}