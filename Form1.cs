using System;

namespace JeuDeCombat
{
    public partial class Form1 : Form
    {
        //Variable pour l'UI du menu
        Button Valider;
        List<string> classe = new List<string> { "Damager", "Healer", "Tank" };
        string classeJ1;
        string classeJ2;
        bool isClassChoose = false;
        List<string> actions = new List<string> { "Attaquer", "Défendre", "Action Spé" };
        List<string> difficulte = new List<string>() { "1", "2", "3", "4"};
        int difficultyChoose;
        int manche = 0;
        int PvJoueur;
        int maxPvJoueur;
        string actionsJ1 = "";
        string actionsJ2 = "";
        int PvOrdi;
        int maxPvOrdi;
        bool firstRound = true;

        public Form1()
        {
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            Validate();
            Valider.Click += new EventHandler(ChooseDifficulty);
            textClass.Text = "Veuillez choisir une classe.";
            PutButton(classe, buttonChoose);
        }

        //Fait apparaitre le bouton valider
        private void Validate()
        {
            Valider = new Button();
            Controls.Add(Valider);
            Valider.Text = "Valider";
            Valider.Location = new Point(350, 400);
            
        }
        //Faire apparaitre les différents boutons de choix
        private void PutButton(List<string> ListOfText, EventHandler actionToDo)
        {
            for (int i = 0; i < ListOfText.Count; i++)
            {
                RadioButton buttonClasse = new RadioButton();
                choiceButtons.Controls.Add(buttonClasse);
                buttonClasse.Location = new Point(25 + 150 * i, 25);
                buttonClasse.Text = ListOfText[i];
                buttonClasse.Click += new EventHandler(actionToDo);
            }
        }

        private void ChooseDifficulty(object sender, EventArgs e)
        {
            choiceButtons.Controls.Clear();
            choiceButtons.Text = "Difficulté";
            textClass.Text = "Choisissez une difficulté";
            Valider.Click -= new EventHandler(ChooseDifficulty);
            Valider.Click += new EventHandler(LaunchBattle);
            PutButton(difficulte, difficultyToChoose);
            
        }
        //Permet de sélectionner une difficulté via les boutons
        private void difficultyToChoose(object sender, EventArgs e)
        {
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi la difficulté " + buttonTook;
            difficultyChoose = int.Parse(buttonTook);
        }
        //Permet de sélectionner une classe via les boutons
        private void buttonChoose(object sender, EventArgs e)
        {
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi " + buttonTook + ".";
            classeJ1 = buttonTook;
            if (!isClassChoose) isClassChoose = true;
        }

        //Gère le lancement du combat
        private void LaunchBattle(object sender, EventArgs e)
        {
            textClass.Text = "Le combat commence";
            choiceButtons.Hide();
            choiceButtons.Controls.Clear();
            choiceButtons.Text = "Action";
            Valider.Hide();
            textClass.Hide();
            textClass.Text = "Choisissez une action.";
            PutButton(actions, actionChoose);
            choiceButtons.Show();
            textClass.Show();
            if (firstRound)
            {
                Random random = new Random();
                Dictionary<string, int> ViePerso = new Dictionary<string, int>();
                Dictionary<string, int> AttaquePerso = new Dictionary<string, int>();
                ViePerso.Add("Damager", 3); ViePerso.Add("Healer", 4); ViePerso.Add("Tank", 5);
                AttaquePerso.Add("Damager", 2); AttaquePerso.Add("Healer", 1); AttaquePerso.Add("Tank", 1);
                PvJoueur = ViePerso[classeJ1];
                maxPvJoueur = PvJoueur;
                int ChoixOrdi = random.Next(1, 3);
                classeJ2 = classe[ChoixOrdi - 1];
                PvOrdi = ViePerso[classeJ2];
                maxPvOrdi = PvOrdi;
                firstRound = false;
            }
            InGame();
            Valider.Show();

        }

        //Gère la création et la mise à jour des stats
        private void Stats(GroupBox StatsBox, int pv, string classPlayer, string lastAction)
        {
            StatsBox.Controls.Clear();
            List<string> statsList = new List<string> { "PV : ", "Classe : ", "Dernière action : ", lastAction };
            List<string> statsString = new List<string>() { pv.ToString(), classPlayer, "", "" };
            
            for (int i = 0; i < statsList.Count; i++)
            {
                Label statsText= new Label();
                StatsBox.Controls.Add(statsText);
                statsText.Location = new Point(5, 25*i+25);
                statsText.Text = statsList[i] + statsString[i];
            }
            StatsBox.Show();
        }
        //Permet la création et le choix des boutons d'attaques
        private void actionChoose(object sender, EventArgs e)
        {
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi " + buttonTook + ".";
            actionsJ1 = buttonTook;
        }

        //Gère le combat
        private void InGame()
        {
            manche++;
            name.Text = "Manche " + manche;
            name.TextAlign = ContentAlignment.TopCenter;
            Stats(playerStat, PvJoueur, classeJ1, actionsJ1);
            Stats(ordiStats, PvOrdi, classeJ2, actionsJ2);
            Random random = new Random();
            switch (difficultyChoose)
            {
                case 0:
                    {// do nothing
                        //actionsJ2 = 4;
                    }
                    break;
                case 1:
                    {// attack when weak, protect when strong (k that dumb)
                        bool willSpecial = random.Next(1, 4) == 4;
                        if (willSpecial)
                            actionsJ2 = "Action Spé";
                        else if (PvJoueur < PvOrdi)
                            actionsJ2 = "Attaquer";
                        else
                            actionsJ2 = "Défendre";
                    }
                    break;
                case 2:
                    {// random
                        actionsJ2 = actions[random.Next(1, 3)];
                    }
                    break;
                case 3:
                    {// protect when weak, attack when strong
                        bool willSpecial = random.Next(1, 4) == 4;
                        if (willSpecial)
                            actionsJ2 = "Action Spé";
                        else if (PvJoueur > PvOrdi)
                            actionsJ2 = "Attaquer";
                        else
                            actionsJ2 = "Défendre";
                    }
                    break;
                case 4:
                    {// smarter
                        float ratioOrdi = PvOrdi / maxPvOrdi;
                        float ratioJoueur = PvJoueur / maxPvJoueur;

                        int probaSpecialPercent = 10;
                        int probaAttackPercent = 50;
                        probaSpecialPercent += (int)Math.Round(ratioOrdi * 100);
                        probaSpecialPercent += (int)Math.Round((ratioOrdi - ratioJoueur) * 100);

                        if (random.Next(100) < probaSpecialPercent)
                            actionsJ2 = "Attaque Spé";
                        else if (random.Next(100) < probaAttackPercent)
                            actionsJ2 = "Attaquer";
                        else
                            actionsJ2 = "Défendre";
                    }
                    break;
                default:
                    {

                    }
                    break;
            }
            bool rageJ = false;
            bool rageO = false;
            //Verification Rage
            if (actionsJ1 == "Action Spé" && classeJ1 == "Tank") rageJ = true;
            if (actionsJ2 == "Action Spé" && classeJ2 == "Tank") rageO = true;
            //Joueur
            //Spécial Tank
            if (actionsJ1 == "Action Spé" && classeJ1 == "Tank")
            {
                PvJoueur--;
                if (actionsJ2 != "Défendre")
                {
                    PvOrdi -= 2;
                    if (rageO)
                    {
                        PvJoueur -= 2;
                    }
                }
            }
            //Spécial Healer
            if (actionsJ1 == "Action Spé" && classeJ1 == "Healer")
            {
                PvJoueur += 2;
            }
            //Attaques
            if (actionsJ1 == "Attaquer")
            {
                if (actionsJ2 != "Défendre")
                {
                    PvOrdi--;
                    if (rageO)
                    {
                        PvJoueur--;
                    }
                }
            }

            //ORDINATEUR
            //Spécial Tank
            if (actionsJ1 == "Action Spé" && classeJ2 == "Tank")
            {
                PvOrdi--;
                if (actionsJ1 != "Défendre")
                {
                    PvJoueur -= 2;
                    if (rageJ)
                    {
                        PvOrdi -= 2;
                    }
                }
            }
            //Spécial Healer
            if (actionsJ2 == "Action Spé" && classeJ2 == "Healer")
            {
                PvOrdi += 2;
            }
            //Attaques
            if (actionsJ2 == "Attaquer")
            {
                if (actionsJ1 != "Défendre")
                {
                    PvJoueur--;
                    if (rageJ)
                    {
                        PvOrdi--;
                    }
                }
            }
            //Empêche d'avoir plus de pv que le max (pour les healers)
            if (classeJ1 == "Healer" && PvJoueur > 4) PvJoueur = 4;
            if (classeJ2 == "Healer" && PvOrdi > 4) PvJoueur = 4;

            Stats(playerStat, PvJoueur, classeJ1, actionsJ1);
            Stats(ordiStats, PvOrdi, classeJ2, actionsJ2);
            if (PvJoueur <= 0 || PvOrdi <= 0) Interface_Fin(PvJoueur, PvOrdi);
        }

        //Gère la fin du match
        private void Interface_Fin(int PvJoueur, int PvOrdi)
        {
            name.Hide();
            choiceButtons.Hide();
            playerStat.Hide();
            ordiStats.Hide();
            textClass.Hide();
            Valider.Click += new EventHandler (endForm);
            if (PvJoueur <= 0 && PvOrdi <= 0)
            {
                name.Text = "Vous avez fait égalité.";
            }
            else if (PvJoueur <= 0)
            {
                name.Text = "Vous avez perdu.";
            }
            else if (PvOrdi <= 0)
            {
                name.Text = "Vous avez gagné.";
            }
            name.TextAlign = ContentAlignment.TopCenter;
            name.Show();
        }

        private void endForm(object sender, EventArgs e)
        {
            Close();
        }

    }
}
