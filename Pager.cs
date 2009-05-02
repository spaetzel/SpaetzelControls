using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Spaetzel.Controls
{

    public class Pager : CompositeControl
    {
        private uint? _curPage;

        public uint NumPages { get; set; }


        public uint CurPage
        {
            get
            {
                return (uint)_curPage;
            }
            set
            {
               
                _curPage = (uint?)value;

                this.Visible = (value > 1);

  

            }

        }
        public uint LinksToShow { get; set; }

        private string _curPageUrl = null;
        private string CurPageUrl
        {
            get
            {
                if (_curPageUrl == null)
                {
                    string url = HttpContext.Current.Request.RawUrl.ToLower();

                    var questionIndex = url.IndexOf("?");

                    if (questionIndex > 0)
                    {
                        url = url.Substring(0, questionIndex);
                    }

                    _curPageUrl = url;



                    //foreach( var curElement in HttpContext.Current.Request.qu
                }

                return _curPageUrl;
            }
        }

        public Pager()
        {
            NumPages = 1;
            

            LinksToShow = 12;
        }

      
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            uint renderPage;

            if (CurPage < NumPages - 3 || NumPages < LinksToShow)
            {
                for (renderPage = Math.Max(CurPage - 3, 1); renderPage < (LinksToShow - 4 + CurPage) && renderPage <= NumPages; renderPage++)
                {
                    this.Controls.Add(GenerateLink(renderPage));

                    Literal spacer = new Literal();
                    spacer.Text = "&nbsp;";
                    this.Controls.Add(spacer);
                }

                if (renderPage < NumPages)
                {
                    Literal elipsis = new Literal();
                    elipsis.Text = "...&nbsp;";
                    this.Controls.Add(elipsis);

                    for (renderPage = NumPages - 1; renderPage <= NumPages; renderPage++)
                    {
                        this.Controls.Add(GenerateLink(renderPage));

                        Literal spacer = new Literal();

                        if (renderPage <= NumPages - 1)
                        {
                            spacer.Text = "&nbsp;";
                            this.Controls.Add(spacer);
                        }
                    }
                }
            }

            else
            {
                for (renderPage = 1; renderPage <= 3 && renderPage <= NumPages; renderPage++)
                {
                    this.Controls.Add(GenerateLink(renderPage));

                    Literal spacer = new Literal();
                    spacer.Text = "&nbsp;";
                    this.Controls.Add(spacer);
                }

                if (renderPage < NumPages)
                {
                    Literal elipsis = new Literal();
                    elipsis.Text = "...&nbsp;";
                    this.Controls.Add(elipsis);

                    for (renderPage = (NumPages - ( LinksToShow - 4 ) ); renderPage <= NumPages; renderPage++)
                    {
                        this.Controls.Add(GenerateLink(renderPage));

                        Literal spacer = new Literal();
                        spacer.Text = "&nbsp;";
                        this.Controls.Add(spacer);
                    }
                }
            }
                
        }

        private Control GenerateLink(uint pageNum)
        {
            if (pageNum != CurPage)
            {
                HyperLink output = new HyperLink();
                output.Text = pageNum.ToString();

                /*
                string url = Context.Request.Url.AbsoluteUri;

                int pageIndex = url.IndexOf("page=");

                if (pageIndex > 1)
                {
                    if (url[pageIndex - 1] == '&' || url[pageIndex-1] == '?')
                        pageIndex--;
                }


                string strippedUrl;
                if (pageIndex > 0)
                {
                    int endIndex = Math.Min(url.IndexOf('&', pageIndex+1), url.Length);
                    if (endIndex <= 0)
                    {
                        endIndex = url.Length;
                    }

                    strippedUrl = Context.Request.Url.AbsoluteUri.Remove(pageIndex, endIndex - pageIndex);
                }
                else
                {
                    strippedUrl = url;
                }

                char queryChar;

                if (strippedUrl.Contains('?'))
                    queryChar = '&';
                else
                    queryChar = '?';

                output.NavigateUrl = strippedUrl + queryChar + "page=" + pageNum.ToString();
                */

                output.NavigateUrl = CurPageUrl + "?page=" + pageNum.ToString();

                return output;
            }
            else
            {
                Literal output = new Literal();
                output.Text = CurPage.ToString();

                return output;
            }

        }

    }
}
