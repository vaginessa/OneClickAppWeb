using System;
using Mono.Data.Sqlite;

using CoreFoundation;
using CoreGraphics;
using UIKit;
using Foundation;
using QuickLook;

using Arshu.Web.Common;
using Arshu.AppWeb;

namespace App.Secure
{
    [Register("WebViewController")]
    public class WebViewController : UIViewController
    {
        #region Variable

        public NSUrl _appUri;
        private static ArshuWebGrid _arshuWebGrid = null;        
        private float heightOffset = 0; //25.0f;

        #endregion

		#region Constructor

        public WebViewController()
        {
            _arshuWebGrid = new ArshuWebGrid(this);
            SetNeedsStatusBarAppearanceUpdate();
			DummyObjectRegister ();
        }

		#endregion

		#region Basic Overrides

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();
        }

		/// <summary>
		/// Views the did load.
		/// </summary>
        public override void ViewDidLoad()
        {
			InitWebGrid ();

            base.ViewDidLoad();
        }

		#endregion

		#region Rotation Overrides

		public override void WillRotate (UIInterfaceOrientation toInterfaceOrientation, double duration)
		{
			Console.WriteLine("WillRotate! " + UIDevice.CurrentDevice.Orientation);

            ReloadWebGrid();

			base.WillRotate (toInterfaceOrientation, duration);
		}

		#endregion




        #region Init WebGrid

        /// <summary>
		/// Inits the web grid.
		/// </summary>
		/// <param name="rootView">Root view.</param>
		private void InitWebGrid()
		{
            DummyObjectRegister();
            UIView rootView = this.View;
			if (_arshuWebGrid != null)
			{
                CGSize screenSize = _arshuWebGrid.GetScreenSize();
                _arshuWebGrid.InitView(rootView, screenSize.Width, screenSize.Height, heightOffset); 

				_arshuWebGrid.CurrentPageAnimation = PageAnimation.FlipRight;
                _arshuWebGrid.RequireWifi = false;
				_arshuWebGrid.StartAnimationTime = 2000;
				_arshuWebGrid.EndAnimationTime = 1000;
                _arshuWebGrid.ShowInstallLink = true;
                _arshuWebGrid.ShowBackLink = true;
                _arshuWebGrid.RestartOnRotate = false;
                _arshuWebGrid.UseDocumentFolder = true;
			}
		}

		#endregion

        #region Start WebGrid

        public void StartWebGrid()
        {
            if (_arshuWebGrid != null)
            {
                _arshuWebGrid.ConfigView();

                ConfigureWebView();
                ImportApp();

                _arshuWebGrid.StartWebServer(false);

               
            }
        }

        #endregion

        #region Stop WebGrid

        public void StopWebGrid()
        {
            if (_arshuWebGrid != null)
            {
                _arshuWebGrid.StopWebServer();
            }
        }

        #endregion

        #region Reload WebGrid

        public void ReloadWebGrid()
        {
            InitWebGrid();

            if (_arshuWebGrid != null)
            {
                _arshuWebGrid.ConfigView();
                _arshuWebGrid.ReloadView();
                ConfigureWebView();
            }
        }

        #endregion

        #region Import App

        private void ImportApp()
        {
            if (_arshuWebGrid != null)
            {
                if (_appUri != null)
                {
                    _arshuWebGrid.ImportApp(_appUri.AbsoluteString);
                    _appUri = null;
                }
            }
        }

        #endregion

        #region Configure WebView

        private void ConfigureWebView()
        {
            if (_arshuWebGrid != null)
            {
                if (_arshuWebGrid.MainWebView != null)
                {
                    // if this is false, page will be 'zoomed in' to normal size
                    _arshuWebGrid.MainWebView.ScalesPageToFit = true;

                    //_arshuWebGrid.MainWebView.UserInteractionEnabled = false;
                    //_arshuWebGrid.MainWebView.ScrollView.ScrollEnabled = false;
                    _arshuWebGrid.MainWebView.ScrollView.BouncesZoom = false;
                    _arshuWebGrid.MainWebView.ScrollView.Bounces = false;
                }
            }
        }

        #endregion

        #region Dummy Register

        //Dummy Variable to Suppress the error could not create a dempendency map for System.Xml
		System.Xml.XmlDocument _xmlDocument = null;
		bool _isNull = false;
		private void DummyObjectRegister()
		{
			//To Remove the error could not create a complete dempendency map
			if ((_arshuWebGrid == null) && (_isNull==true)) {
				_xmlDocument = new System.Xml.XmlDocument ();
			}

			if ((_xmlDocument == null) && (_isNull ==false)) {
				_isNull = true;
			}

            SqliteFactory sqliteFactory = new SqliteFactory();
            if ((sqliteFactory == null) && (_isNull == false))
            {
                _isNull = true;
            }
		}

		#endregion

        #region Hide Status Bar

        public override bool PrefersStatusBarHidden()
        {
            return true;
        }

        #endregion

        #region Quick Look Url

        //private class PreviewItem : QLPreviewItem
        //{
        //    public string Title { get; set; }
        //    public NSUrl Url { get; set; }

        //    public override NSUrl ItemUrl { get { return Url; } }
        //    public override string ItemTitle { get { return Title; } }
        //}

        //private class PreviewDataSource : QLPreviewControllerDataSource
        //{
        //    private NSUrl _url;
        //    private string _title;

        //    public PreviewDataSource(NSUrl url, string title)
        //    {
        //        _url = url;
        //        _title = title;
        //    }

        //    public override int PreviewItemCount(QLPreviewController controller)
        //    {
        //        return 1;
        //    }

        //    public override QLPreviewItem GetPreviewItem(QLPreviewController controller, int index)
        //    {
        //        return new PreviewItem { Title = _title, Url = _url };
        //    }
        //}

        //public bool QuickLookUrl(NSUrl documentUrl, string title)
        //{
        //    bool ret = false;
        //    QLPreviewController qlPreview = new QLPreviewController();
        //    qlPreview.DataSource = new PreviewDataSource(documentUrl, title);
        //    this.PresentViewController(qlPreview, true, null);
        //    ret = true;

        //    return ret;
        //}

        //private UIDocumentInteractionController _popup;
        //private void ShareUrl(NSUrl fileUrl)
        //{
        //    if (_popup != null)
        //    {
        //        _popup.DismissMenu(true);
        //        _popup = null;
        //    }
        //    else
        //    {
        //        _popup = new UIDocumentInteractionController()
        //        {
        //            Url = fileUrl
        //        };
        //        _popup.DidDismissOptionsMenu += (s, a) => _popup = null;
        //        _popup.PresentOptionsMenu(_shareButton, true);
        //    }
        //}

        #endregion
    }
}