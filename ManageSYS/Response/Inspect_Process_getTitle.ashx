﻿<%@ WebHandler Language="C#" Class="Inspect_Process_getTitle" %>

using System;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;

public class Inspect_Process_getTitle : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        System.Diagnostics.Debug.WriteLine("getTitle 收到请求");

        byte[] postData = new byte[context.Request.InputStream.Length];
        context.Request.InputStream.Read(postData, 0, postData.Length);
        string postDataString = System.Text.Encoding.UTF8.GetString(postData);
        JObject js = (JObject)JsonConvert.DeserializeObject(postDataString);
        String prodcode = js["prodcode"].ToString();
        var json = new JObject();
        var titles = new JArray();
        //string sql = "select r.inspect_code,s.name as insgroup,r.inspect_name,0 as value,t.lower_value,t.upper_value,r.unit,'' as status,t.minus_score from ht_qlt_inspect_proj r left join ht_inner_inspect_group s on s.id = r.inspect_group left join ht_qlt_inspect_stdd t on t.inspect_code = r.inspect_code and t.is_del = '0' where r.inspect_group in('1','2','3') and r.is_del = '0' order by r.inspect_code";
        string sql = "select r.inspect_code,s.section_name ,r.inspect_name,0 as value,t.lower_value,t.upper_value,r.unit,'' as status,t.minus_score from ht_qlt_inspect_proj r left join ht_pub_tech_section s on s.section_code = r.inspect_group left join ht_qlt_inspect_stdd_sub t on t.inspect_code = r.inspect_code and t.is_del = '0' left join  ht_pub_prod_design ss on t.stdd_code = ss.inspect_stdd where r.inspect_type = '0'  and r.is_del = '0' and ss.prod_code = '"+prodcode+"' order by r.inspect_code";
        MSYS.DAL.DbOperator opt = new MSYS.DAL.DbOperator();
        DataSet data = opt.CreateDataSetOra(sql);
        //string[] title = new string[data.Tables[0].Rows.Count];
        for (int i = 0; i < data.Tables[0].Rows.Count; i++)
        {
            // title[i] = data.Tables[0].Rows[i][2].ToString();
            titles.Add(data.Tables[0].Rows[i][2].ToString());
        }
        json.Add("titles", titles);
        //context.Response.ContentType = "text/plain";
        context.Response.ContentType = "json";
        context.Response.Write(json.ToString());
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}