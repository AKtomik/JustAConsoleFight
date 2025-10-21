using System;
using Microsoft.VisualBasic.Devices;

namespace JeuDeCombat
{
    public class Settings
    {
    }
    
    public class Character(int maxHp, int atk)
	{
        public static readonly Dictionary<string, Character> all = new()
        {
            {"Damager", new Character(maxHp: 3, atk: 2)},
            {"Healer", new Character(maxHp: 4, atk: 1)},
            {"Tank", new Character(maxHp: 5, atk: 1)},
            {"Goblin", new Character(maxHp: 3, atk: 1)},
        };

        //public static string[] = Character.all.Keys.ToArray();
        
        // utilisation des constructeurs primaires
        public int maxHp = maxHp;
        public int atk = atk;
	}


	public class Player
    {
        // character & round
        public string characterName;
        public Character character;
        public int MaxHp
        {
            get => character.maxHp;
        }
        public int Atk
        {
            get => character.atk;
        }
        public double Ratio
        {
            get => hp / MaxHp;
        }

        Round round;
        int roundIndex;
        public Player Enemy
        {
            get => this.round.player[Math.Abs(roundIndex - 1)];
        }
            
        // atributes
        public int hp = 0;

        // compute
        public bool isHuman = false;
        public int botLevel = 0;

        public string lastAction = "";
        public string nextAction = "";

        // special
        public bool SpecialRage = false;
        public int SpecialGoblinAmount = 1;
        
        // init
        public Player(Round round, int roundIndex)
		{
            this.round = round;
            this.roundIndex = roundIndex;
		}

        public void SetClass(string characterName)
        {
            this.characterName = characterName;
            this.character = Character.all[characterName];
            hp = character.maxHp;
        }
        
        public void SetHuman()
		{
            isHuman = true;
		}
        public void SetBot(int level)
        {
            isHuman = false;
            botLevel = level;
        }

        // tool
        public bool IsCharacter(string CharacterName)
		{
            return Character.all[CharacterName] == character;
		}

        // play
        public void SetNextAction(string action)
        {
            this.nextAction = action;
        }

        public string GetPlayerTurn()
        {
            if (isHuman)
            {
                this.lastAction = this.nextAction;
                return lastAction;
            }
            else
            {
                this.lastAction = ComputerTurn();
                return lastAction;
            }
        }
        
        public string NextPlayerTurn()
        {
            if (isHuman)
            {
                this.lastAction = this.nextAction;
                return lastAction;
            }
            else
			{
                this.lastAction = ComputerTurn();
                return lastAction;
			}
        }
        
        string ComputerTurn()
        {
            switch (botLevel)
            {
                case 0:
                    {// do nothing
                        return "AFK";//for compile
                    }
                case 1:
                    {// attack when weak, protect when stron
                        return "Attaquer";
                    }
                case 2:
                    {// pure random
                        return Form1.AviableActions[Round.random.Next(3)];
                    }
                case 3:
                    {// use ratio to think
                        int probaSpecialPercent = (int)Math.Round(this.Ratio * 100);
                        int probaAttackPercent = (int)Math.Round((this.Ratio - Enemy.Ratio) * 100);

                        if (Round.random.Next(100) < probaSpecialPercent)
                            return "Attaque Spé";
                        else if (Round.random.Next(100) < probaAttackPercent)
                            return "Attaquer";
                        else
                            return "Défendre";
                    }
                case 4:
                    {// smarter
                        if (Enemy.lastAction == "Attaquer") return "Défendre";
                        if (Enemy.lastAction == "Défendre") return "Attaque Spé";
                        if (Enemy.lastAction == "Attaque Spé") return "Attaquer";
                        return "Défendre";
                    }
                default:
                    {
                        return "AFK";//for compile
                    }
            }
		}
    }
    
    public class Round
    {

        public int turn = 0;
        public static Random random = new Random();
        public Player[] player;

        public Round()
        {
            player = [new(this, 0), new(this, 1)];
		}

        public void Start()
        {
        }
        
        // take the fight. Return -1 if need to continue. Return -2 if equality. Else return the player index (0/1)
        public int Fight()
        {
            string[] playersActions = [player[0].GetPlayerTurn(), player[1].GetPlayerTurn()];

            turn++;

            //Verification Rage
            if (playersActions[0] == "Action Spé" && player[0].IsCharacter("Tank")) player[0].SpecialRage = true;
            if (playersActions[1] == "Action Spé" && player[1].IsCharacter("Tank")) player[1].SpecialRage = true;
            //Joueur
            //Spécial Tank
            if (playersActions[0] == "Action Spé" && player[0].IsCharacter("Tank"))
            {
                player[0].hp--;
                if (playersActions[1] != "Défendre")
                {
                    player[1].hp -= 2;
                    if (player[1].SpecialRage)
                    {
                        player[0].hp -= 2;
                    }
                }
                else
                {
                    player[1].hp -= 1;
                }
            }
            //Spécial Healer
            if (playersActions[0] == "Action Spé" && player[0].IsCharacter("Healer"))
            {
                player[0].hp += 2;
            }
            //Attaques
            if (playersActions[0] == "Attaquer")
            {
                if (playersActions[1] != "Défendre")
                {
                    if (player[0].IsCharacter("Damager"))
                    {
                        player[1].hp -= 2;
                    }
                    if (player[0].IsCharacter("Goblin"))
                    {
                        player[1].hp -= 1*player[0].SpecialGoblinAmount;
                    }
                    else
                    {
                        player[1].hp--;
                    }
            
                    if (player[1].SpecialRage)
                    {
                        player[0].hp--;
                    }
                }
            }
            
            //ORDINATEUR
            //Spécial Tank
            if (playersActions[0] == "Action Spé" && player[1].IsCharacter("Tank"))
            {
                player[1].hp--;
                if (playersActions[0] != "Défendre")
                {
                    player[0].hp -= 2;
                    if (player[0].SpecialRage)
                    {
                        player[1].hp -= 2;
                    }
                }
                else
                {
                    player[0].hp -= 1;
                }
            }
            //Spécial Healer
            if (playersActions[1] == "Action Spé" && player[1].IsCharacter("Healer"))
            {
                player[1].hp += 2;
            }
            //Attaques
            if (playersActions[1] == "Attaquer")
            {
                if (playersActions[0] != "Défendre")
                {
                    if (player[1].IsCharacter("Damager"))
                    {
                        player[0].hp -= 2;
                    }
                    if (player[1].IsCharacter("Goblin"))
                    {
                        player[0].hp -= 1*player[1].SpecialGoblinAmount;
                    }
                    else
                    {
                        player[0].hp--;
                    }
            
                    if (player[0].SpecialRage)
                    {
                        player[1].hp--;
                    }
                }
            }
            //Empêche d'avoir plus de pv que le max (pour les healers)
            if (player[0].IsCharacter("Healer") && player[0].hp > 4) player[0].hp = 4;
            if (player[1].IsCharacter("Healer") && player[1].hp > 4) player[0].hp = 4;


            if (player[0].hp <= 0 && player[1].hp <= 0) return -2;
            if (turn >= 100) return -3;
            if (player[0].hp <= 0) return 0;
            if (player[1].hp <= 0) return 1;
            return -1;
        }
    }

    public partial class Form1 : Form
    {
        //Variable pour les listes de choix
        public static readonly string[] AviableCharacters = ["Damager", "Healer", "Tank", "Goblin"];
        public static readonly string[] AviableActions = ["Attaquer", "Défendre", "Action Spé"];
        public static readonly string[] AviableDifficulties = ["1", "2", "3", "4"];

        //Variable pour l'UI du menu
        Button ValidButton = new();
        Button[] MenuButtons = [new(), new(), new()];

        //Variables pour la détection du choix
        string classChoice;
        string actionChoice;
        int difficultyChoice;
        string classBot1Choice;
        string classBot2Choice;
        int levelBot1Choice;
        int levelBot2Choice;

        //Variables pour la manche
        Round round;

        public Form1()
        {
            InitializeComponent();
            CreateValidate();
            CreateMenu();
        }

        // UTILITAIRE

        //Créer le bouton valider
        private void CreateValidate()
        {
            Controls.Add(ValidButton);
            ValidButton.Text = "Valider";
            ValidButton.Location = new Point(350, 400);
        }

        //Créer le menu
        private void CreateMenu()
        {
            Controls.Add(MenuButtons[0]);
            Controls.Add(MenuButtons[1]);
            Controls.Add(MenuButtons[2]);
            MenuButtons[0].Text = "Jouer";
            MenuButtons[0].Location = new Point(200, 400);
            MenuButtons[0].Click += PlayGame;
            MenuButtons[1].Text = "Simuler";
            MenuButtons[1].Location = new Point(350, 400);
            MenuButtons[1].Click += PlaySimulate;
            MenuButtons[2].Text = "Quitter";
            MenuButtons[2].Location = new Point(500, 400);
            MenuButtons[2].Click += Close;
        }

        private EventHandler _currentHandler;
        private void SetValidate(EventHandler newEvent)
        {
            if (_currentHandler != null) ValidButton.Click -= _currentHandler;
            _currentHandler = newEvent;
            ValidButton.Click += newEvent;
        }

        private void SetText(string message)
        {
            textClass.Text = message;
        }
        private void SetTitle(string message)
        {
            name.Text = message;
        }

        //Faire apparaitre les différents boutons de choix
        private void PutRadio(string[] ListOfText, EventHandler actionToDo)
        {
            choiceButtons.Show();
            choiceButtons.Controls.Clear();
            for (int i = 0; i < ListOfText.Length; i++)
            {
                RadioButton buttonClasse = new RadioButton();
                choiceButtons.Controls.Add(buttonClasse);
                buttonClasse.Location = new Point(25 + 120 * i, 25);
                buttonClasse.Text = ListOfText[i];
                buttonClasse.Click += new EventHandler(actionToDo);
            }
        }

        // FONCTIONS MENU
        // les fonctions forment une chaine et sont dans l'odre

        // étape menu principal
        private void Form1_Load(object? sender, EventArgs e)
        {
            Menu(sender, e);
        }
        
        private void Close(object? sender, EventArgs e)
        {
            Close();
        }

        private void Menu(object? sender, EventArgs e)
        {
            CreateMenu();
            MenuButtons.ToList().ForEach(v => v.Show());
            SetText("Bienvenue !");
            SetTitle("Super Street Combat 74 Deluxe Premium +");
            choiceButtons.Hide();
            ValidButton.Hide();
        }

        private void PlayGame(object? sender, EventArgs e)
        {
            MenuButtons.ToList().ForEach(v => v.Hide());
            textClass.Show();
            CharacterChoose(sender, e);
        }

        private void PlaySimulate(object? sender, EventArgs e)
        {
            MenuButtons.ToList().ForEach(v => v.Hide());
            textClass.Show();
            Bot1ClassChoose(sender, e);
        }
        
        // PARAMÈTRES JEU
        // les fonctions forment une chaine et sont dans l'odre



        // étape de sélection de la classe
        private void CharacterChoose(object? sender, EventArgs e)
        {

            choiceButtons.Text = "Classe";
            SetText("Veuillez choisir une classe.");
            PutRadio(AviableCharacters, CharacterPick);
            SetValidate(DifficultyChoose);
            ValidButton.Hide();
        }

        //Permet de sélectionner une classe via les boutons
        private void CharacterPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi " + buttonTook + ".";
            classChoice = buttonTook;
            ValidButton.Show();
        }

        // étape de sélection de la difficulté
        private void DifficultyChoose(object? sender, EventArgs e)
        {
            choiceButtons.Text = "Difficulté";
            textClass.Text = "Choisissez une difficulté";
            PutRadio(AviableDifficulties, DifficultyPick);
            SetValidate(PlayRound);
            ValidButton.Hide();
        }

        //Permet de sélectionner une difficulté via les boutons
        private void DifficultyPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi la difficulté " + buttonTook;
            difficultyChoice = int.Parse(buttonTook);
            ValidButton.Show();
        }

        // JOUER LA MANCHE
        // les fonctions Play() sont des boucles

        // lancer une manche
        private void PlayRound(object? sender, EventArgs e)
        {
            // round init
            round = new Round();

            // human player set
            round.player[0].SetHuman();
            round.player[0].SetClass(classChoice);

            // bot player set
            int ClassInt = Round.random.Next(Character.all.Keys.Count) + 1;
            string classeChoisieBot = Character.all.Keys.ToArray()[ClassInt - 1];
            round.player[1].SetBot(difficultyChoice);
            round.player[1].SetClass(classeChoisieBot);

            // ui
            TurnUiUpdate();
            SetValidate(PlayTurn);
            ValidButton.Hide();
        }

        // prohcain tour
        private void PlayTurn(object? sender, EventArgs e)
        {
            int winner = round.Fight();
            if (winner != -1)
            {
                VictoryUi(winner);
                return;
            }

            // ui
            TurnUiUpdate();
            //name.Text = "winner:" + winner;

            ValidButton.Show();
        }

        // monter l'interface du choix d'action
        private void TurnUiUpdate()
        {
            name.Text = "Tour " + (round.turn + 1);
            choiceButtons.Hide();
            choiceButtons.Text = "Action";
            textClass.Hide();
            textClass.Text = "Choisissez une action.";
            PutRadio(AviableActions, ActionPick);
            choiceButtons.Show();
            textClass.Show();
            ValidButton.Hide();

            Stats(playerStat, round.player[0].hp, round.player[0].characterName, round.player[0].lastAction);
            Stats(ordiStats, round.player[1].hp, round.player[1].characterName, round.player[1].lastAction);
        }

        //Permet la création et le choix des boutons d'attaques
        private void ActionPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            actionChoice = classr.Text;
            textClass.Text = "Vous avez choisi " + actionChoice + ".";
            round.player[0].SetNextAction(actionChoice);
            ValidButton.Show();
        }

        //Gère la création et la mise à jour des stats
        private void Stats(GroupBox StatsBox, int pv, string classPlayer, string lastAction)
        {
            StatsBox.Controls.Clear();
            List<string> statsList = new List<string> { "PV : ", "Classe : ", "Dernière action : ", lastAction };
            List<string> statsString = new List<string>() { pv.ToString(), classPlayer, "", "" };

            for (int i = 0; i < statsList.Count; i++)
            {
                Label statsText = new Label();
                StatsBox.Controls.Add(statsText);
                statsText.Location = new Point(5, 25 * i + 25);
                statsText.Text = statsList[i] + statsString[i];
            }
            StatsBox.Show();
        }

        //Gère la fin du match
        private void VictoryUi(int winner)
        {
            name.Hide();
            choiceButtons.Hide();
            playerStat.Hide();
            ordiStats.Hide();
            textClass.Hide();
            SetValidate(Menu);
            if (winner == -2)
            {
                name.Text = "Vous avez fait égalité.";
            }
            else if (winner == 1)
            {
                name.Text = "Vous avez perdu.";
            }
            else if (winner == 0)
            {
                name.Text = "Vous avez gagné.";
            }
            name.TextAlign = ContentAlignment.TopCenter;
            name.Show();
        }


        // PARAMÈTRES SIMULATION

        // étape de sélection de la classe du bot 1
        private void Bot1ClassChoose(object? sender, EventArgs e)
        {
            SetText("Veuillez choisir une classe pour le bot 1.");
            choiceButtons.Text = "Classe";
            PutRadio(AviableCharacters, Bot1ClassPick);
            SetValidate(Bot1LevelChoose);
            ValidButton.Hide();
        }

        //Permet de sélectionner une classe via les boutons
        private void Bot1ClassPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi " + buttonTook + " pour le bot 1.";
            classBot1Choice = buttonTook;
            ValidButton.Show();
        }

        // étape de sélection du niveau du bot 1
        private void Bot1LevelChoose(object? sender, EventArgs e)
        {
            SetText("Veuillez choisir une difficulté pour le bot 1.");
            choiceButtons.Text = "Difficulté";
            PutRadio(AviableDifficulties, Bot1LevelPick);
            SetValidate(Bot2ClassChoose);
            ValidButton.Hide();
        }

        //Permet de sélectionner un niveau via les boutons
        private void Bot1LevelPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi la difficulté " + buttonTook + ".";
            levelBot1Choice = int.Parse(buttonTook);
            ValidButton.Show();
        }

        // étape de sélection de la classe du bot 2
        private void Bot2ClassChoose(object? sender, EventArgs e)
        {
            SetText("Veuillez choisir une classe pour le bot 2.");
            choiceButtons.Text = "Classe";
            PutRadio(AviableCharacters, Bot2ClassPick);
            SetValidate(Bot2LevelChoose);
            ValidButton.Hide();
        }

        //Permet de sélectionner une classe via les boutons
        private void Bot2ClassPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi " + buttonTook + " pour le bot 2.";
            classBot2Choice = buttonTook;
            ValidButton.Show();
        }

        // étape de sélection du niveau du bot 2
        private void Bot2LevelChoose(object? sender, EventArgs e)
        {
            SetText("Veuillez choisir une difficulté pour le bot 2.");
            choiceButtons.Text = "Difficulté";
            PutRadio(AviableDifficulties, Bot2LevelPick);
            SetValidate(Simulate);
            ValidButton.Hide();
        }

        //Permet de sélectionner un niveau via les boutons
        private void Bot2LevelPick(object? sender, EventArgs e)
        {
            if (sender == null) return;
            RadioButton classr = (RadioButton)sender;
            String buttonTook = classr.Text;
            textClass.Text = "Vous avez choisi la difficulté " + buttonTook + ".";
            levelBot2Choice = int.Parse(buttonTook);
            ValidButton.Show();
        }

        // Do the simulation
        private void Simulate(object? sender, EventArgs e)
        {
            int winBot1 = 0;
            int winBot2 = 0;
            int winSteal = 0;

            
            for (int i = 0; i < 100; i++)
			{
                // round init
                round = new Round();
                round.player[0].SetBot(levelBot1Choice);
                round.player[0].SetClass(classBot1Choice);
                round.player[1].SetBot(levelBot2Choice);
                round.player[1].SetClass(classBot2Choice);
                
                int winner = -1;
                while (winner == -1)
                {
                    winner = round.Fight();
                }
                
                if (winner == -2 || winner == -3)
                    winSteal += 1;
                else if (winner == 0)
                    winBot1 += 1;
                else if (winner == 1)
                    winBot2 += 1;
			}

            
            name.Hide();
            choiceButtons.Hide();
            playerStat.Hide();
            ordiStats.Hide();
            textClass.Hide();
            SetValidate(Menu);
            name.Text = "Bot 1 : "+winBot1+" Bot 2 : "+winBot2+" égal : "+winSteal;
            name.TextAlign = ContentAlignment.TopCenter;
            name.Show();
        }
    }
}

