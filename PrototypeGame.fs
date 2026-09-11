module PrototypeFighter.Scripts.PrototypeGame

open FGScript

let Create options =
    def GAME {
        optimize options
        counterStun 2<frames>
        punishStun 4<frames>
        stopFrames CounterHit 1<frames>
        stopFrames Punish 2<frames>

        entities [ Bot.Main.main ]
        entities Bot.Projectiles.main
        auras [ Vfx.Auras.Armor; Vfx.Auras.White; Vfx.Auras.Fire; Vfx.Auras.EX ]
        effects Vfx.all

        sparks Hit {
            Small = Vfx.HitSpark.Small, Sfx.Hit.Light
            Medium = Vfx.HitSpark.Medium, Sfx.Hit.Medium
            Large = Vfx.HitSpark.Large, Sfx.Hit.Heavy
            Special = Vfx.HitSpark.Special, Sfx.Hit.Heavy
        }

        sparks Block {
            Small = Vfx.HitSpark.Block, Sfx.Block.Light
            Medium = Vfx.HitSpark.Block, Sfx.Block.Medium
            Large = Vfx.HitSpark.Block, Sfx.Block.Heavy
            Special = Vfx.HitSpark.Block, Sfx.Block.Heavy
        }

        sparks CounterHit {
            Small = Vfx.HitSpark.CounterSmall, Sfx.Hit.Medium
            Medium = Vfx.HitSpark.CounterMedium, Sfx.Hit.Heavy
            Large = Vfx.HitSpark.CounterLarge, Sfx.Hit.Counter
            Special = Vfx.HitSpark.CounterSpecial, Sfx.Hit.Counter
        }

        sparks Punish {
            Small = Vfx.HitSpark.PunishSmall, Sfx.Hit.Medium
            Medium = Vfx.HitSpark.PunishMedium, Sfx.Hit.Heavy
            Large = Vfx.HitSpark.PunishLarge, Sfx.Hit.Counter
            Special = Vfx.HitSpark.PunishSpecial, Sfx.Hit.Counter
        }

        sparksArmor Vfx.HitEffect.Armor Sfx.Hit.Armor
        sparksArmorBreak Vfx.HitEffect.ArmorBreak Sfx.Hit.ArmorBreak
        sparksParry Vfx.HitEffect.ArmorBreak Sfx.Hit.Parry
    }
