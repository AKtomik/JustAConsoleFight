using System;

public class Program
{
	static int IntPut(int inputMinIncluded, int inputMaxIncluded)
	{
		while (true)
		{
			string? inputString = Console.ReadLine();
			int inputInt;
			Console.WriteLine("");
			if (int.TryParse(inputString, out inputInt) && inputInt >= inputMinIncluded && inputInt <= inputMaxIncluded)
				return inputInt;
		}
	}



	static void Main()
	{
		// Définition variables
		Random random = new Random();
		Dictionary<string, int> ViePerso = new Dictionary<string, int>();
		Dictionary<string, int> AttaquePerso = new Dictionary<string, int>();
		ViePerso.Add("Damager", 3); ViePerso.Add("Healer", 4); ViePerso.Add("Tank", 5);
		AttaquePerso.Add("Damager", 2); AttaquePerso.Add("Healer", 1); AttaquePerso.Add("Tank", 1);
		int manche = 1;
		int PvJoueur = 0, maxPvJoueur = 0;

		/////////////////////
		// Interface Début //
		/////////////////////

		// Introduction au jeu
		Console.WriteLine("+------------------------+");
		Console.WriteLine("| Jeu de combat de fou ! |");
		Console.WriteLine("+------------------------+");
		Console.WriteLine("");

		// Choix du personnage
		List<string> personnages = new List<string> {"Damager", "Healer", "Tank"};
		Console.WriteLine("Faites un choix :");
		Console.WriteLine("1 - Damager");
		Console.WriteLine( "2 - Healer");
		Console.WriteLine("3 - Tank");
		Console.Write("Reponse : ");
		bool ErreurChoix = true;
		while (ErreurChoix == true)
		{
			int ChoixPerso = Convert.ToInt32(Console.ReadLine());

			Console.WriteLine("");
			if (ChoixPerso == 1 || ChoixPerso == 2 || ChoixPerso == 3)
			{
				Console.WriteLine("Tu as choisi un {0}", personnages[ChoixPerso - 1]);
				maxPvJoueur = ViePerso[personnages[ChoixPerso - 1]];
				PvJoueur = maxPvJoueur;
				ErreurChoix = false;
			}
			else
			{
				Console.WriteLine("Erreur choix");
			}
		}
		
		// Choix difficulté
		Console.WriteLine("niveau de l'adversaire :");
		Console.WriteLine("1 à 4 graduel");
		int ordiDifficulty = IntPut(0, 4);

		// Introduction au combat / Choix ordinateur
		int ChoixOrdi = random.Next(1, 3);
		Console.WriteLine("Tu vas afronter un {0} niveau {1}", personnages[ChoixOrdi - 1], ordiDifficulty);
		int maxPvOrdi = ViePerso[personnages[ChoixOrdi - 1]];
		int PvOrdi = maxPvOrdi;
		Console.WriteLine("");
		System.Threading.Thread.Sleep(1111);

		//////////////////////
		// Interface Manche //
		//////////////////////

		while (PvJoueur > 0 && PvOrdi > 0)
		{
			// Affichage Numéro Manche
			string tiret = "";
			int ChfManche = manche;
			while (ChfManche != 0) { ChfManche = ChfManche / 10; tiret = tiret + "-"; }

			Console.WriteLine("+---------{0}+", tiret);
			Console.WriteLine("| Manche {0} |", manche);
			Console.WriteLine("+---------{0}+", tiret);
			Console.WriteLine("");

			// Affichage Combat
			string espace = "";
			Console.WriteLine("JOUEUR	  {0}	  ADVERSAIRE", espace);
			Console.WriteLine("[{0}PV]1	  {1}		  2[{2}PV]", PvJoueur, espace, PvOrdi);
			Console.WriteLine("");

			// Affichage Actions
			List<string> actions = new List<string> { "Attaquer", "Défendre", "Action Spéciale", "Regarder sans rien faire" };
			Console.WriteLine("Actions possibles :");
			Console.WriteLine("1 - Attaquer");
			Console.WriteLine("2 - Défendre");
			Console.WriteLine("3 - Action Spéciale");
			Console.Write("Choix : ");

			// Action joueur
			bool ErreurAction = true;
			while (ErreurAction)
			{
				int ActionPerso = Convert.ToInt32(Console.ReadLine());

				Console.WriteLine("");
				if (ActionPerso == 1 || ActionPerso == 2 || ActionPerso == 3)
				{
					Console.WriteLine("Tu as choisi {0}", actions[ActionPerso - 1]);
					ErreurAction = false;
				}
				else
				{
					Console.WriteLine("Erreur choix");
				}
			}
			System.Threading.Thread.Sleep(1111);

			// Action ordi
			int ActionOrdi = 0;
			switch (ordiDifficulty)
			{
				case 0:
					{// do nothing
						ActionOrdi = 4;
					} break;
				case 1:
					{// attack when weak, protect when strong (k that dumb)
						bool willSpecial = random.Next(1, 4) == 4;
						if (willSpecial)
							ActionOrdi = 3;
						else if (PvJoueur < PvOrdi)
							ActionOrdi = 1;
						else
							ActionOrdi = 2;
					} break;
				case 2:
					{// random
						ActionOrdi = random.Next(1, 3);
					} break;
				case 3:
					{// protect when weak, attack when strong
						bool willSpecial = random.Next(1, 4) == 4;
						if (willSpecial)
							ActionOrdi = 3;
						else if (PvJoueur > PvOrdi)
							ActionOrdi = 1;
						else
							ActionOrdi = 2;
					} break;
				case 4:
					{// smarter
						float ratioOrdi = PvOrdi / maxPvOrdi;
						float ratioJoueur = PvJoueur / maxPvJoueur;
						
						int probaSpecialPercent = 10;
						int probaAttackPercent = 50;
						probaSpecialPercent += (int)Math.Round(ratioOrdi*100);
						probaSpecialPercent += (int)Math.Round((ratioOrdi - ratioJoueur)*100);
						
						if (random.Next(100) < probaSpecialPercent)
							ActionOrdi = 3;
						else if (random.Next(100) < probaAttackPercent)
							ActionOrdi = 1;
						else
							ActionOrdi = 2;
					} break;
				default:
					{
						
					} break;
			}
			Console.WriteLine("L'adversaire a choisi {0}", actions[ActionOrdi - 1]);
			System.Threading.Thread.Sleep(1111);

			// CALCUL PV TEMPO
			// TODO : CALCUL DES ATTAQUES
			// UTILISER ACTIONPERSO ET ACTIONS ORDI POUR INFLUENCER LES PV
			PvJoueur--;
			PvOrdi--;
			manche++;
		}
		
		///////////////////
		// Interface Fin //
		///////////////////
		Console.WriteLine("");
		Interface_Fin(PvJoueur, PvOrdi);
	}
	static void Interface_Fin(int PvJoueur, int PvOrdi)
	{
		if (PvJoueur <= 0 && PvOrdi <= 0)
		{
			Console.WriteLine("Egalité");
		}
		else if (PvJoueur <= 0)
		{
			Console.WriteLine("Défaite");
		}
		else if (PvOrdi <= 0)
		{
			Console.WriteLine("Victoire");
		}
	}
}
