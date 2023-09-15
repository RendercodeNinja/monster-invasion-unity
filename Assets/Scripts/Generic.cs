/// ---------------------------------------------------------------------
/// File: Generic.cs
/// Project: Monster Invasion
/// Author: RendercodeNinja
/// Description: Custom data type defenitions within the project scope
/// Target: None
/// Instances: N/A
/// ---------------------------------------------------------------------

using System;

namespace MonsterInvasion.Generic
{
    /// <summary>
    /// Represents a class supporting Integer min/max values
    /// </summary>
    [Serializable]
    public class Range
    {
        public int min, max;
    }

    /// <summary>
    /// Represents a class supporting Floating point min/max values
    /// </summary>
    [Serializable]
    public class RangeF
    {
        public float min, max;
    }

    /// <summary>
    /// Enum defining the available Monster color types
    /// </summary>
    public enum MonsterColor
    {
        Blue,
        Red
    }

    /// <summary>
    /// Enum defining states of the Monster characer
    /// </summary>
    public enum MonsterStates
    {
        Inert,
        PlatformMotion,
        DroppingDown,
        FloorMotion
    }

    /// <summary>
    /// Enum defining the types of available particles
    /// </summary>
    public enum ParticleTypes
    {
        ProjectileLost,
        ProjectileHit,
        PlayerExplode
    }

    /// <summary>
    /// Enum defining the states of the game
    /// </summary>           
    public enum GameStates
    {
        NonPlayable,
        Playable
    }

    /// <summary>
    /// Enum defining the states of the UI
    /// </summary>
    public enum UIStates
    {
        InGame,
        GameOver
    }
}
