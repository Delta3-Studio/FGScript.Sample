module PrototypeFighter.Scripts.Bot.BotAI

open FGScript

let main =
    def BEHAVIOR {
        parameter "aggression" [
            Difficult.Easy => 30<percent>
            Difficult.Medium => 45<percent>
            Difficult.Hard => 65<percent>
        ]

        parameter "guard" [
            Difficult.Easy => 20<percent>
            Difficult.Medium => 40<percent>
            Difficult.Hard => 60<percent>
        ]

        parameter "throw" [
            Difficult.Easy => 8<percent>
            Difficult.Medium => 14<percent>
            Difficult.Hard => 20<percent>
        ]

        def NODE "Root" {
            selector

            nodes {
                chance &&"guard" "ChooseGuard"
                chance &&"throw" "TryThrow"
                "CloseCombat"
                "Turtle"
                "MidCombat"
                "Approach"
                "Wait"
            }
        }

        def NODE "ChooseGuard" {
            neutralOnly
            maxDistance 150
            condition Var.Entity.inProximityGuard

            execute { log "AI: Doing guard" }

            nodes {
                chance 80<percent> "CrouchGuard"
                "StandGuard"
            }
        }

        def NODE "StandGuard" { execute { input B 15<times> } }
        def NODE "CrouchGuard" { execute { input DB 15<times> } }

        def NODE "TryThrow" {
            neutralOnly
            maxDistance 50
            execute { input (LP + LK) }
        }

        def NODE "CloseCombat" {
            neutralOnly
            maxDistance 90

            nodes {
                chance 20<percent> "AntiAir"
                chance 10<percent> "WalkBack"
                "HeavyAttack"
                "MediumAttack"
                "QuickAttack"
            }
        }

        def NODE "MidCombat" {
            neutralOnly
            minDistance 90
            maxDistance 200

            nodes {
                chance &&"aggression" "AntiAir"
                chance 18<percent> "Tatsu"
                chance 20<percent> "Fireball"
                chance 15<percent> "DonkeyKick.EX"
                "Advance"
            }
        }

        def NODE "HeavyAttack" {
            chance &&"aggression"

            nodes {
                chance 10<percent> "Sweep"
                chance 15<percent> "CrouchForward"
                chance 20<percent> "HeavyPunchHold"
                chance 20<percent> "HeavyKick"
                "HeavyPunch"
            }
        }

        def NODE "HeavyKick" { execute { input HK } }
        def NODE "CrouchForward" { execute { input D MK } }
        def NODE "HeavyPunch" { execute { input HP } }
        def NODE "HeavyPunchHold" { execute { input HP 45<times> } }

        def NODE "MediumAttack" {
            chance 55<percent>
            execute { input MP }
        }

        def NODE "Sweep" {
            chance 15<percent>
            execute { input D HK }
        }

        def NODE "QuickAttack" { execute { input LP } }

        def NODE "AntiAir" {
            stance Grounded
            condition (opponent (Var.Entity.isAirborne .& !Var.Entity.inStunLike))

            execute {
                log "AI: Doing anti-air"
                input DP HP
            }
        }

        def NODE "Fireball" {
            execute {
                wait 12<frames>
                input QCF MP
            }
        }

        def NODE "DonkeyKick.EX" {
            condition (res r.Energy .>= 100)
            execute { input !B F KK }
        }

        def NODE "Tatsu" { execute { input HCB LK } }

        def NODE "Advance" { execute { input F 5<times> } }

        def NODE "DiveKick" {
            stance Airborne
            execute { input QCB HK }
        }

        def NODE "Approach" {
            neutralOnly
            minDistance 200

            nodes {
                chance 16<percent> "JumpIn"
                chance 25<percent> "DashForward"
                "WalkForward"
            }
        }

        def NODE "JumpIn" {
            neutralOnly
            weighted

            execute {
                input UF
                wait 14<frames>
            }

            nodes {
                weight 6 "JumpIn.HK"
                weight 4 "DiveKick"
            }
        }

        def NODE "JumpIn.HK" {
            stance Airborne

            execute {
                wait 12<frames>
                input HK
            }
        }

        def NODE "Turtle" {
            chance 20<percent>
            execute { input DB 16<times> }
        }

        def NODE "WalkForward" { execute { input F 8<times> } }
        def NODE "WalkBack" { execute { input B 8<times> } }
        def NODE "DashForward" { execute { input F F } }
        def NODE "Wait" { execute { frame } }
    }
