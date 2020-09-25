using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.VersionControl.Client;


namespace SpellFixTool
{
    public partial class Form1 : Form
    {

        private const string tfsServer = @"http://tfspro.atlanta.hp.com:8080/tfs";
        private const string filename = @"C:\ES_AMERICAS_US_ADU_RI_DMV\Branches\Dev1\dev\src\sln\Presentation\prj\CommonPresentationLib\Frameworks\UIControl\User Control\CustomUserControl,vb";

        public Form1()
        {
            InitializeComponent();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckOutFromTFS(filename);
        }

        private void CheckOutFromTFS(string fileName)
        {
            using (TfsTeamProjectCollection pc = TfsTeamProjectCollectionFactory.GetTeamProjectCollection(new Uri(tfsServer)))
            {
                if (pc != null)
                {
                    WorkspaceInfo workspaceInfo = Workstation.Current.GetLocalWorkspaceInfo(fileName);
                    if (null != workspaceInfo)
                    {
                        Workspace workspace = workspaceInfo.GetWorkspace(pc);
                        workspace.PendEdit(fileName);
                    }
                }
            }
         }

        private void FindAndReplace()
        {

            File.WriteAllText(filename, File.ReadAllText(filename).Replace("some text", "some other text"), GetEncoding(filename));

        }

        /// <summary>
        /// Determines a text file's encoding by analyzing its byte order mark (BOM).
        /// Defaults to ASCII when detection of the text file's endianness fails.
        /// </summary>
        /// <param name="filename">The text file to analyze.</param>
        /// <returns>The detected encoding.</returns>
        private Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open)) file.Read(bom, 0, 4);

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return Encoding.UTF32;
            return Encoding.ASCII;
        }


    }
}
