public enum ModifierDescriptor
{
    None,
    Buff,
    Debuff,
    Unique,
    Temporary,
    Permanent,
    OneShot, // Modifier does not linger, it applies it's effect and disapears
    OnHit, // Does something when affected character lands a hit in combat
    NoVisual, // Do not create character modifier notifications 
    PlayerCharacterOnly
}