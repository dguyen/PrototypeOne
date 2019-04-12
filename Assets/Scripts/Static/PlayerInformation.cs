using System.Collections;
using System.Collections.Generic;

public static class PlayerInformation {
    private static Player[] Players = new Player[4];

    public static void SetPlayer(int PlayerIndex, Player Player) {
        Players[PlayerIndex] = Player;
    }

    public static Player GetPlayer(int PlayerIndex) {
        return Players[PlayerIndex];
    }

    public static void SetPlayers(Player[] InputPlayers) {
        Players = InputPlayers;
    }

    public static Player[] GetPlayers() {
        return Players;
    }
}

public class Player {
    public int PlayerNumber;
    public int PlayerControlNumber;
    public int SelectedCharacter;
}
