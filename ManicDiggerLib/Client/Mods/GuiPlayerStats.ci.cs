﻿public class ModGuiPlayerStats : ClientMod
{
    public override void OnNewFrameDraw2d(Game game, float deltaTime)
    {
        healthPosX = game.Width() / 2 - baseSizeX - centerOffset;
        healthPosY = game.Height() - 115;
        oxygenPosX = game.Width() / 2 + centerOffset;
        oxygenPosY = game.Height() - 115;

        if (game.guistate != GuiState.MapLoading)
        {
            DrawPlayerHealth(game);
            DrawPlayerOxygen(game);
        }
    }

    //Sizes of Health/Oxygen bar
    const int baseSizeX = 220;
    const int baseSizeY = 16;
    const int centerOffset = 20;
    const int barSpacing = 2;

    int healthPosX;
    int healthPosY;
    int oxygenPosX;
    int oxygenPosY;

    public void DrawPlayerHealth(Game game)
    {
        if (game.PlayerStats != null)
        {
            float progress = game.one * game.PlayerStats.CurrentHealth / game.PlayerStats.MaxHealth;
            game.Draw2dTexture(game.GetTexture("ui_bar_background.png"), healthPosX, healthPosY, baseSizeX, baseSizeY, null, 0, Game.ColorFromArgb(255, 255, 255, 255), false);
            game.Draw2dTexturePart(game.GetTexture("ui_bar_inner.png"), progress, 1, healthPosX + barSpacing, healthPosY + barSpacing, (progress) * (baseSizeX - (2 * barSpacing)), baseSizeY - (2 * barSpacing), Game.ColorFromArgb(255, 255, 0, 0), false);
        }
    }

    public void DrawPlayerOxygen(Game game)
    {
        if (game.PlayerStats != null)
        {
            if (game.PlayerStats.CurrentOxygen < game.PlayerStats.MaxOxygen)
            {
                float progress = game.one * game.PlayerStats.CurrentOxygen / game.PlayerStats.MaxOxygen;
                game.Draw2dTexture(game.GetTexture("ui_bar_background.png"), oxygenPosX, oxygenPosY, baseSizeX, baseSizeY, null, 0, Game.ColorFromArgb(255, 255, 255, 255), false);
                game.Draw2dTexturePart(game.GetTexture("ui_bar_inner.png"), progress, 1, oxygenPosX + barSpacing, oxygenPosY + barSpacing, (progress) * (baseSizeX - (2 * barSpacing)), baseSizeY - (2 * barSpacing), Game.ColorFromArgb(255, 0, 0, 255), false);
            }
        }
    }
}
