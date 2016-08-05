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
            _problems.Add(new Problem47());
            _problems.Add(new Problem48());
            _problems.Add(new Problem49());
            _problems.Add(new Problem50());
            _problems.Add(new Problem51());
            _problems.Add(new Problem52());
            _problems.Add(new Problem53());
            _problems.Add(new Problem54());
            _problems.Add(new Problem55());
            _problems.Add(new Problem56());
            _problems.Add(new Problem57());
            _problems.Add(new Problem58());
            _problems.Add(new Problem59());
            _problems.Add(new Problem60());
            _problems.Add(new Problem61());
            _problems.Add(new Problem62());
            _problems.Add(new Problem63());
            _problems.Add(new Problem64());
            _problems.Add(new Problem65());
            _problems.Add(new Problem66());
            _problems.Add(new Problem67());
            _problems.Add(new Problem68());
            _problems.Add(new Problem69());
            _problems.Add(new Problem70());
            _problems.Add(new Problem71());
            _problems.Add(new Problem72());
            _problems.Add(new Problem73());
            _problems.Add(new Problem74());
            _problems.Add(new Problem75());
            _problems.Add(new Problem76());
            _problems.Add(new Problem77());
            _problems.Add(new Problem78());
            _problems.Add(new Problem79());
            _problems.Add(new Problem80());
            _problems.Add(new Problem81());
            _problems.Add(new Problem82());
            _problems.Add(new Problem83());
            _problems.Add(new Problem84());
            _problems.Add(new Problem85());
            _problems.Add(new Problem87());
            _problems.Add(new Problem89());
            _problems.Add(new Problem91());
            _problems.Add(new Problem92());
            _problems.Add(new Problem95());
            _problems.Add(new Problem96());
            _problems.Add(new Problem97());
            _problems.Add(new Problem99());
            _problems.Add(new Problem102());
            _problems.Add(new Problem104());
            _problems.Add(new Problem108());
            _problems.Add(new Problem112());
            _problems.Add(new Problem124());
            _problems.Add(new Problem125());
            _problems.Add(new Problem145());
            _problems.Add(new Problem205());
            _problems.Add(new Problem206());
            _problems.Add(new Problem301());
            _problems.Add(new Problem306());
            _problems.Add(new Problem345());
            _problems.Add(new Problem357());
            _problems.Add(new Problem387());
            _problems.Add(new Problem493());
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
