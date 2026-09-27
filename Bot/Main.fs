module PrototypeFighter.Scripts.Bot.Main

open FGScript

let main =
    def CHARACTER "FightBot" {
        name "Fight Bot"
        archetype Archetype.AllRounder PlayStyle.Traditional MoveStyle.Motion
        health 1000
        chipDamage 25<percent> Special Super
        animationPrefix "bot/"
        tags Tag.None

        hurtVoice Light v.Hurt.Light
        hurtVoice Medium v.Hurt.Medium
        hurtVoice Heavy v.Hurt.Heavy
        hurtVoice CounterHit v.Hurt.Counter

        load Base.states
        load Events.states
        load Hurts.states
        load KnockDowns.states
        load Movement.states
        load Normals.states
        load Throws.states
        load Specials.states
        load BotAI.main

        def RESOURCE r.Stamina {
            max 500
            init 500
        }

        def RESOURCE r.Energy {
            max 300
            init 100
            keepBetweenRounds
            gainOnBlock 50<percent>
            giveOnBlock 25<percent>
            giveOnHit 70<percent>
        }
    }
