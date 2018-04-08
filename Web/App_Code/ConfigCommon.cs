using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using System.Configuration;

/// <summary>
///ConfigCommon 的摘要说明
/// </summary>
public class ConfigCommon
{
    public ConfigCommon()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }


    /// <summary>
    /// 获取导航轮播图片xml节点的值
    /// </summary>
    /// <param name="XmlName">xml文件名称</param>
    /// <param name="Id">ID名称</param>
    /// <param name="NodeName">要获取节点名称</param>
    /// <returns></returns>
    public static string GetBanner(string XmlName, string Id, string NodeName)
    {
        string NodeValue = string.Empty;
        XmlDocument xml = null;
        if (HttpRuntime.Cache.Get("AllNewBanner") == null)
        {
            xml = new XmlDocument();
            string filename = HttpContext.Current.Server.MapPath("~/Config/" + XmlName); //你的xml地址
            if (File.Exists(filename))
            {
                xml.Load(filename);
                HttpRuntime.Cache.Insert("AllNewBanner", xml, null, DateTime.Now.AddMinutes(3), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }
        else
        {
            xml = HttpRuntime.Cache["AllNewBanner"] as XmlDocument;
        }
        if (xml != null)
        {
            foreach (XmlNode node in xml.ChildNodes)
            {
                if (node.Name == "root")
                {
                    foreach (XmlNode node1 in node.ChildNodes)
                    {
                        if (node1.Name == "img")
                        {
                            foreach (XmlNode node2 in node1.ChildNodes)
                            {
                                if (node2.Name == "id" && node2.InnerText == Id)
                                {
                                    switch (NodeName)
                                    {
                                        case "id":
                                            if (node1.SelectSingleNode("id") != null)
                                            {
                                                NodeValue = node1.SelectSingleNode("id").InnerText;
                                            }
                                            break;
                                        case "title":
                                            if (node1.SelectSingleNode("title") != null)
                                            {
                                                NodeValue = node1.SelectSingleNode("title").InnerText;
                                            }
                                            break;
                                        case "url":
                                            if (node1.SelectSingleNode("url") != null)
                                            {
                                                NodeValue = node1.SelectSingleNode("url").InnerText;
                                            }
                                            break;
                                        case "remark":
                                            if (node1.SelectSingleNode("remark") != null)
                                            {
                                                NodeValue = node1.SelectSingleNode("remark").InnerText;
                                            }
                                            break;
                                        case "imgurl":
                                            if (node1.SelectSingleNode("imgurl") != null)
                                            {
                                                NodeValue = node1.SelectSingleNode("imgurl").InnerText;
                                            }
                                            break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
        return NodeValue;
    }


    /// <summary>
    /// 首页配置
    /// </summary>
    /// <param name="XmlName">Xml配置文件名称</param>
    /// <param name="ElementName">配置节点名称</param>
    /// <returns></returns>
    public static string GetIndexConfig(string XmlName, string ElementName)
    {
        string WriteHtml = "";
        XmlDocument xml = null;
        XmlNodeList NodeList = null;
        XmlNode ChildNode = null;
        try
        {
            xml = new XmlDocument();
            string filename = HttpContext.Current.Server.MapPath("~/Config/" + XmlName); //你的xml文件名称
            if (File.Exists(filename))
            {
                xml.Load(filename);
                XmlNode ElenmentNode = xml.SelectSingleNode("root");
                if (ElenmentNode == null)
                {
                    return "";
                }
                switch (ElementName)
                {
                    #region  首页Banner图配置
                    case "IndexBanner":
                        NodeList = ElenmentNode.SelectNodes(ElementName);
                        if (NodeList != null && NodeList.Count > 0)
                        {
                            foreach (XmlNode Node in NodeList[0].ChildNodes)
                            {
                                ChildNode = Node;
                                WriteHtml += "<div class=\"piece clearfix\">";
                                WriteHtml += "<a href = \"" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "\" target = \"_blank\" class=\"img-b\" data-bg-col=\"" + ChildNode.SelectSingleNode("bgColor").InnerText.Trim() + "\"><img src =\"/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "\" data-src2=\"/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "\"  class=\"slide_lazy\" data-id=\"super-slide\" /></a>";
                                WriteHtml += "</div>";
                            }
                        }
                        ; break;
                    #endregion

                    #region  优品推荐配置

                    case "ProductRecommend":
                        NodeList = ElenmentNode.SelectNodes(ElementName);
                        if (NodeList != null && NodeList.Count > 0)
                        {
                            ChildNode = NodeList[0].SelectSingleNode("Img1");
                            if (ChildNode != null)
                            {
                                WriteHtml += "<a target='_blank' class='Optima1 fl' href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                WriteHtml += "<h4>" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h4><p>查看详情</p>";
                                WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' width='168' height='168'/>";
                                WriteHtml += "</a>";
                            }
                            ChildNode = NodeList[0].SelectSingleNode("Img2");
                            if (ChildNode != null)
                            {
                                WriteHtml += "<a target='_blank' class='Optima2 fl' href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                WriteHtml += "<h4>" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h4><p>查看详情</p>";
                                WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' width='168' height='168'/>";
                                WriteHtml += "</a>";
                            }
                            ChildNode = NodeList[0].SelectSingleNode("Img3");
                            if (ChildNode != null)
                            {
                                WriteHtml += "<div class='fl you'>";
                                WriteHtml += "<a target='_blank' class='Optima3' href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' width='168' height='168' class='fr'/>";
                                WriteHtml += "<h4>" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h4><p>查看详情</p>";
                                WriteHtml += "</a>";
                            }
                            ChildNode = NodeList[0].SelectSingleNode("Img4");
                            if (ChildNode != null)
                            {
                                WriteHtml += "<a target='_blank' class='Optima4' href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' width='168' height='168' class='fr'/>";
                                WriteHtml += "<h4>" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h4><p>查看详情</p>";
                                WriteHtml += "</a></div>";
                            }
                            else
                            {
                                WriteHtml += "</div>";
                            }
                        }
                        ; break;

                    #endregion

                    #region 优惠促销

                    case "ProductPromotion":
                        NodeList = ElenmentNode.SelectNodes(ElementName);
                        if (NodeList != null && NodeList.Count > 0)
                        {
                            foreach (XmlNode Node in NodeList[0].ChildNodes)
                            {
                                ChildNode = Node;
                                WriteHtml += "<li>";
                                WriteHtml += "<a href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' width='160' height='160'>";
                                
                                //WriteHtml += "<h4 class='t'>" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h4><p class='t2'>" + ChildNode.SelectSingleNode("Title").InnerText.Trim() + "</p>";

                                WriteHtml += "<h4 class='t'>" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h4><p class='t2'></p>";
                                WriteHtml += "</a>";
                               
                                //WriteHtml += "<div class='cx-btn'><a class='red-bth' href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>查看详情</a><a class='blue-btn' href='javascript:void(0)'>我要代理</a></div>";

                                WriteHtml += "<div class='cx-btn'><a class='red-bth' href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>查看详情</a></div>";
                                WriteHtml += "</li>";
                            }
                        }
                        ; break;

                    #endregion

                    #region  家用常备

                    case "HomeStandby":
                        NodeList = ElenmentNode.SelectNodes(ElementName);
                        if (NodeList != null && NodeList.Count > 0)
                        {
                            NodeList = NodeList[0].SelectNodes("Left");

                            //左边第一幅图的配置
                            if (NodeList != null && NodeList.Count > 0)
                            {
                                ChildNode = NodeList[0].ChildNodes[0];
                                if (ChildNode != null)
                                {
                                    WriteHtml += "<div class=\"jiayong fl\">";
                                    WriteHtml += "<a href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                    WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' height='366' />";
                                    WriteHtml += "<div class=\"b\"><span class=\"redbtn\">包邮</span><span class=\"bluebtn\">全国联保</span></div>";
                                    WriteHtml += "<h3 class=\"t\">" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</h3>";
                                    WriteHtml += "<p class=\"huise\">" + ChildNode.SelectSingleNode("Title").InnerText.Trim() + "</p>";
                                    WriteHtml += "</a>";
                                    WriteHtml += "</div>";
                                }
                            }


                            //右边列表图的配置

                            NodeList = ElenmentNode.SelectNodes(ElementName)[0].SelectNodes("Right");
                            if (NodeList != null && NodeList.Count > 0)
                            {
                                WriteHtml += "<div class=\"fl jiayonglist\"><ul>";
                                foreach (XmlNode Node in NodeList[0].ChildNodes)
                                {
                                    ChildNode = Node;
                                    WriteHtml += "<li>";
                                    WriteHtml += "<a href='" + (ChildNode.SelectSingleNode("Url").InnerText.Trim() == "" ? "javascript:void(0)" : ChildNode.SelectSingleNode("Url").InnerText.Trim()) + "'>";
                                    WriteHtml += "<img src='/Config/image/" + ChildNode.SelectSingleNode("ImgName").InnerText.Trim() + "' width=\"256\" height=\"200\"  /><p class=\"t\">" + ChildNode.SelectSingleNode("Name").InnerText.Trim() + "</p>";
                                    WriteHtml += "</a>";
                                    WriteHtml += "</li>";
                                }
                                WriteHtml += "</ul></div>";
                            }

                        }
                        ; break;

                        #endregion

                }
            }
        }
        catch (Exception ex)
        {

        }
        return WriteHtml;
    }
    /// 获取指定节点下面对应属性的值
    /// </summary>
    /// <param name="strFileName">文件路径</param>
    /// <param name="nodeName">节点名称</param>
    /// <param name="nodeDir">指定节点所在的节点目录</param>
    /// <param name="attribute">节点对应的属性名称</param>
    /// <returns></returns>
    public static string GetNodeValue(string XmlName, string nodeName)
    {
        XmlDocument xml = null;//xml文档对象 获取真个xml文档
        XmlNode ChildNode = null;//
        try
        {

            string filename = HttpContext.Current.Server.MapPath("~/Config/" + XmlName); //你的xml文件名称
            if (File.Exists(filename))
            {
                xml = new XmlDocument();
                xml.Load(filename);
                XmlElement ElenmentNode = xml.DocumentElement;//xml 文档的根节点
                ChildNode = ElenmentNode.SelectSingleNode(nodeName);//获取第一个匹配的子节点
                return ChildNode.SelectSingleNode("Value").InnerText;//获取第一个匹配的子节点的值
            }
        }
        catch (Exception exp)
        {
            throw exp;
        }
        return "";
    }
}