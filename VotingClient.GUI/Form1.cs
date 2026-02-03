using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Grpc.Net.Client;

using VotingSystem.Voting;
using VotingSystem.Registration;

namespace VotingClient.GUI
{
    public partial class Form1 : Form
    {
        private TextBox txtAR;
        private TextBox txtAV;

        private TextBox txtCC;
        private Button btnEmitirCredencial;
        private TextBox txtCredencial;
        private TextBox txtLogAR;

        private Button btnCarregarCandidatos;
        private ListBox lstCandidatos;
        private Button btnVotar;
        private TextBox txtLogAV;

        private Button btnResultados;
        private ListBox lstResultados;

        public Form1()
        {
            BuildUi();

            txtAR.Text = "http://localhost:9093";
            txtAV.Text = "http://localhost:9091";
        }

        private void BuildUi()
        {
            Text = "Voting System - UI";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1000, 650);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Padding = new Padding(10),
            };
            root.RowStyles.Add(new RowStyle(SizeType.Absolute, 70));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            Controls.Add(root);

            var endpoints = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 4,
                RowCount = 1,
                Padding = new Padding(0),
            };
            endpoints.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60));  
            endpoints.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));  
            endpoints.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60)); 
            endpoints.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

            var lblAR = new Label { Text = "AR URL:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            txtAR = new TextBox { Name = "txtAR", Dock = DockStyle.Fill };

            var lblAV = new Label { Text = "AV URL:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft };
            txtAV = new TextBox { Name = "txtAV", Dock = DockStyle.Fill };

            endpoints.Controls.Add(lblAR, 0, 0);
            endpoints.Controls.Add(txtAR, 1, 0);
            endpoints.Controls.Add(lblAV, 2, 0);
            endpoints.Controls.Add(txtAV, 3, 0);

            var gbEndpoints = new GroupBox { Text = "Endpoints", Dock = DockStyle.Fill };
            gbEndpoints.Controls.Add(endpoints);
            root.Controls.Add(gbEndpoints, 0, 0);

            var tabs = new TabControl { Dock = DockStyle.Fill };
            var tabAR = new TabPage("Registo (AR)");
            var tabAV = new TabPage("Votação (AV)");
            var tabAA = new TabPage("Apuramento (AA)");
            tabs.TabPages.Add(tabAR);
            tabs.TabPages.Add(tabAV);
            tabs.TabPages.Add(tabAA);

            BuildTabAR(tabAR);
            BuildTabAV(tabAV);
            BuildTabAA(tabAA);

            root.Controls.Add(tabs, 0, 1);
        }

        private void BuildTabAR(TabPage tab)
        {
            tab.Padding = new Padding(10);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3,
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 160));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); 
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); 
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); 

            var pnlRow1 = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3 };
            pnlRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pnlRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 10));
            pnlRow1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170));

            txtCC = new TextBox { Name = "txtCC", Dock = DockStyle.Fill };
            txtCC.Height = 28;
            btnEmitirCredencial = new Button
            {
                Name = "btnEmitirCredencial",
                Text = "Emitir Credencial",
                AutoSize = false,
                Width = 170,
                Height = 28,
                Anchor = AnchorStyles.Right | AnchorStyles.Top
            };
            btnEmitirCredencial.Click += btnEmitirCredencial_Click;


            pnlRow1.Controls.Add(txtCC, 0, 0);
            pnlRow1.Controls.Add(new Panel(), 1, 0);
            pnlRow1.Controls.Add(btnEmitirCredencial, 2, 0);

            layout.Controls.Add(new Label { Text = "Cartão de Cidadão:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            layout.Controls.Add(pnlRow1, 1, 0);

            txtCredencial = new TextBox { Name = "txtCredencial", Dock = DockStyle.Fill };
            layout.Controls.Add(new Label { Text = "Credencial emitida:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 1);
            layout.Controls.Add(txtCredencial, 1, 1);

            txtLogAR = new TextBox
            {
                Name = "txtLogAR",
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 10)
            };
            layout.Controls.Add(new Label { Text = "Log:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.TopLeft }, 0, 2);
            layout.Controls.Add(txtLogAR, 1, 2);

            tab.Controls.Add(layout);
        }

        private void BuildTabAV(TabPage tab)
        {
            tab.Padding = new Padding(10);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 3
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));

            btnCarregarCandidatos = new Button
            {
                Name = "btnCarregarCandidatos",
                Text = "Carregar Candidatos",
                Dock = DockStyle.Left,
                Width = 170
            };
            btnCarregarCandidatos.Click += btnCarregarCandidatos_Click;

            var pnlTopRight = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            pnlTopRight.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90));
            pnlTopRight.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var txtCredPreview = new TextBox { Dock = DockStyle.Fill, ReadOnly = true };
            tab.Enter += (_, __) => txtCredPreview.Text = txtCredencial?.Text ?? "";

            pnlTopRight.Controls.Add(new Label { Text = "Credencial:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
            pnlTopRight.Controls.Add(txtCredPreview, 1, 0);

            layout.Controls.Add(btnCarregarCandidatos, 0, 0);
            layout.Controls.Add(pnlTopRight, 1, 0);

            lstCandidatos = new ListBox { Name = "lstCandidatos", Dock = DockStyle.Fill };

            txtLogAV = new TextBox
            {
                Name = "txtLogAV",
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                ReadOnly = true,
                Font = new Font("Consolas", 10)
            };

            layout.Controls.Add(lstCandidatos, 0, 1);
            layout.Controls.Add(txtLogAV, 1, 1);

            btnVotar = new Button { Name = "btnVotar", Text = "Votar no candidato selecionado", Dock = DockStyle.Fill };
            btnVotar.Click += (_, __) => txtCredPreview.Text = txtCredencial?.Text ?? "";
            btnVotar.Click += btnVotar_Click;

            layout.Controls.Add(btnVotar, 0, 2);
            layout.SetColumnSpan(btnVotar, 2);

            tab.Controls.Add(layout);
        }

        private void BuildTabAA(TabPage tab)
        {
            tab.Padding = new Padding(10);

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            btnResultados = new Button { Name = "btnResultados", Text = "Obter Resultados", Dock = DockStyle.Left, Width = 160 };
            btnResultados.Click += btnResultados_Click;

            lstResultados = new ListBox { Name = "lstResultados", Dock = DockStyle.Fill };

            layout.Controls.Add(btnResultados, 0, 0);
            layout.Controls.Add(lstResultados, 0, 1);

            tab.Controls.Add(layout);
        }

        private GrpcChannel CreateChannel(string address)
        {
            return GrpcChannel.ForAddress(address);
        }

        private void btnEmitirCredencial_Click(object sender, EventArgs e)
        {
            try
            {
                using var channel = CreateChannel(txtAR.Text.Trim());
                var client = new VoterRegistrationService.VoterRegistrationServiceClient(channel);

                var cc = txtCC.Text.Trim();
                var resp = client.IssueVotingCredential(new VoterRequest { CitizenCardNumber = cc });

                txtLogAR.AppendText($"Elegível: {resp.IsEligible}\r\n");
                txtLogAR.AppendText($"Credencial: {resp.VotingCredential}\r\n\r\n");

                if (!string.IsNullOrWhiteSpace(resp.VotingCredential))
                    txtCredencial.Text = resp.VotingCredential;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro AR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCarregarCandidatos_Click(object sender, EventArgs e)
        {
            try
            {
                using var channel = CreateChannel(txtAV.Text.Trim());
                var client = new VotingService.VotingServiceClient(channel);

                var resp = client.GetCandidates(new GetCandidatesRequest());

                lstCandidatos.Items.Clear();
                foreach (var c in resp.Candidates)
                    lstCandidatos.Items.Add($"{c.Id} - {c.Name}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro AV", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVotar_Click(object sender, EventArgs e)
        {
            try
            {
                if (lstCandidatos.SelectedItem == null)
                {
                    MessageBox.Show("Selecione um candidato.");
                    return;
                }

                var selected = lstCandidatos.SelectedItem.ToString() ?? "";
                var idStr = selected.Split('-')[0].Trim();

                if (!int.TryParse(idStr, out var candidateId))
                {
                    MessageBox.Show("Candidate ID inválido.");
                    return;
                }

                using var channel = CreateChannel(txtAV.Text.Trim());
                var client = new VotingService.VotingServiceClient(channel);

                var cred = txtCredencial.Text.Trim();
                if (string.IsNullOrWhiteSpace(cred))
                {
                    MessageBox.Show("Credencial vazia. Emita a credencial no separador AR.");
                    return;
                }

                var resp = client.Vote(new VoteRequest
                {
                    VotingCredential = cred,
                    CandidateId = candidateId
                });

                txtLogAV.AppendText($"Success: {resp.Success}\r\n");
                txtLogAV.AppendText($"Message: {resp.Message}\r\n\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro Vote", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnResultados_Click(object sender, EventArgs e)
        {
            try
            {
                using var channel = CreateChannel(txtAV.Text.Trim());
                var client = new VotingService.VotingServiceClient(channel);

                var resp = client.GetResults(new GetResultsRequest());

                lstResultados.Items.Clear();
                foreach (var r in resp.Results)
                    lstResultados.Items.Add($"{r.Id} - {r.Name} : {r.Votes}");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro AA", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
