using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Euler_Logic.Problems;

namespace Euler_Win {
    public partial class frmMain : Form {
        private List<IProblem> _problems;

        private void GetProblems() {
            _problems = new List<IProblem>();
            _problems.Add(new Problem1());
            _problems.Add(new Problem2());
            _problems.Add(new Problem3());
            _problems.Add(new Problem4());
            _problems.Add(new Problem5());
            _problems.Add(new Problem6());
            _problems.Add(new Problem7());
            _problems.Add(new Problem8());
            _problems.Add(new Problem9());
            _problems.Add(new Problem10());
            _problems.Add(new Problem11());
            _problems.Add(new Problem12());
            _problems.Add(new Problem13());
            _problems.Add(new Problem14());
            _problems.Add(new Problem15());
            _problems.Add(new Problem16());
            _problems.Add(new Problem17());
            _problems.Add(new Problem18());
            _problems.Add(new Problem19());
            _problems.Add(new Problem20());
            _problems.Add(new Problem21());
            _problems.Add(new Problem22());
            _problems.Add(new Problem23());
            _problems.Add(new Problem24());
            _problems.Add(new Problem25());
            _problems.Add(new Problem26());
            _problems.Add(new Problem27());
            _problems.Add(new Problem28());
            _problems.Add(new Problem29());
            _problems.Add(new Problem30());
            _problems.Add(new Problem31());
            _problems.Add(new Problem32());
            _problems.Add(new Problem33());
            _problems.Add(new Problem34());
            _problems.Add(new Problem35());
            _problems.Add(new Problem36());
            _problems.Add(new Problem37());
            _problems.Add(new Problem38());
            _problems.Add(new Problem39());
            _problems.Add(new Problem40());
            _problems.Add(new Problem41());
            _problems.Add(new Problem42());
            _problems.Add(new Problem43());
            _problems.Add(new Problem44());
            _problems.Add(new Problem45());
            _problems.Add(new Problem46());
            _problems.Add(new Problem48());
            _problems.Add(new Problem49());
            _problems.Add(new Problem55());
            _problems.Add(new Problem58());
            _problems.Add(new Problem61());
            _problems.Add(new Problem62());
            _problems.Add(new Problem63());
            _problems.Add(new Problem67());
            _problems.Add(new Problem74());
            _problems.Add(new Problem75());
            _problems.Add(new Problem76());
            _problems.Add(new Problem78());
            _problems.Add(new Problem92());
            _problems.Add(new Problem301());
            _problems.Add(new Problem306());
        }

        private void PopulateProblems() {
            lstProblems.Items.Clear();
            foreach (IProblem problem in _problems) {
                lstProblems.Items.Add(problem.ProblemName);
            }
            lstProblems.SelectedIndex = 0;
        }

        public frmMain() {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e) {
            GetProblems();
            PopulateProblems();
        }

        private void cmdGo_Click(object sender, EventArgs e) {
            Go();
        }

        private void lstProblems_MouseDoubleClick(object sender, MouseEventArgs e) {
            Go();
        }

        private void Go() {
            if (lstProblems.SelectedIndex > -1) {
                GetProblems();
                IProblem problem = _problems[lstProblems.SelectedIndex];
                Stopwatch watch = new Stopwatch();
                watch.Start();
                txtAnswer.Text = problem.GetAnswer();
                watch.Stop();
                lblTime.Text = watch.Elapsed.TotalSeconds.ToString() + " seconds";
            }
        }
    }
}
