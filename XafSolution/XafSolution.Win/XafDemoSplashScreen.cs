using System;
using System.IO;
using System.Reflection;
using DevExpress.ExpressApp.Win.Utils;
using DevExpress.Utils.Svg;
using DevExpress.XtraSplashScreen;

namespace XafSolution.Win {
    public partial class XafDemoSplashScreen : DemoSplashScreen {
        private void LoadSplashImageFromResource() {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Stream svgStream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Images.SplashScreenImage.svg");
            svgStream.Position = 0;
            pictureEdit2.SvgImage = SvgImage.FromStream(svgStream);
        }
        public XafDemoSplashScreen() {
            InitializeComponent();
            LoadSplashImageFromResource();
        }
        public override void ProcessCommand(Enum cmd, object arg) {
            base.ProcessCommand(cmd, arg);
            if((UpdateSplashCommand)cmd ==UpdateSplashCommand.Description) {
                labelControl2.Text = (string)arg;
            }
        }
    }
}
