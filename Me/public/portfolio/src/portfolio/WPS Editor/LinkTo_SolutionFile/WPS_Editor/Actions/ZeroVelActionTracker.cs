using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WPS_Editor
{
    class ZeroVelActionTracker : WPSActionTracker
    {
        public ZeroVelActionTracker(string name, RefreshingListBox parent, bool isPerm)
        {
            perm = isPerm;
            base.Initialize(name, parent);

            form.Height = 150;

            AddButtons(20);
        }

        protected override void Ok_Clicked()
        {
            base.Ok_Clicked();
        }

        protected override void Apply_Clicked()
        {
            base.Apply_Clicked();
        }

        public override void ShowDialog()
        {
            base.ShowDialog();
        }

        public override string ToXML()
        {
            string xml = "";
            xml += "<action,type=ZeroVelAction>\n" +
                "<property,name=Name,type=string>" + Name + "</property>\n" + AddTemporaryXML() +
                "</action>\n";
            return xml;
        }
    }
}
