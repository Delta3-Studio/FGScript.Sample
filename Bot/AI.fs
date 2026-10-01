module PrototypeFighter.Scripts.Bot.BotAI

open FGScript
open FGScript.Behaviors

let main =
    def BEHAVIOR {
        init Difficult.Easy {
            set "aggression" 30<percent>
            set "guard" 20<percent>
            set "throw" 8<percent>
        }

        init Difficult.Medium {
            set "aggression" 45<percent>
            set "guard" 40<percent>
            set "throw" 14<percent>
        }

        init Difficult.Hard {
            set "aggression" 65<percent>
            set "guard" 60<percent>
            set "throw" 20<percent>
        }

        init () {
            log "AI: default init"
            waitSeconds 1
            wait 10<frames>
        }

        On NEUTRAL { log "AI: in neutral" }
        On DidContact { log "AI: touch opponent" }

        On GotHit {
            If(Var.Combo.hitsTaken == 0)
            log "AI: got hit!"
        }

        On GotThrown {
            Chance &&"guard"
            press (LP + LK)
        }

        def NODE "Root" {
            selector

            nodes {
                chance &&"guard" "ChooseGuard"
                chance &&"throw" "TryThrow"
                "CloseCombat"
                "Turtle"
                "MidCombat"
                "Approach"
            }
        }

        def NODE "ChooseGuard" {
            neutralOnly
            maxDistance 150
            condition Var.Entity.inProximityGuard

            actions { log "AI: Doing guard" }

            nodes {
                chance 80<percent> "CrouchGuard"
                "StandGuard"
            }
        }

        def NODE "StandGuard" { actions { input B 15<times> } }
        def NODE "CrouchGuard" { actions { input DB 15<times> } }

        def NODE "TryThrow" {
            neutralOnly
            maxDistance 65
            actions { input (LP + LK) }
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
            distance 90 200

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

        def NODE "HeavyKick" { actions { input HK } }
        def NODE "CrouchForward" { actions { input D MK } }
        def NODE "HeavyPunch" { actions { input HP } }
        def NODE "HeavyPunchHold" { actions { input HP 45<times> } }

        def NODE "MediumAttack" {
            chance 55<percent>
            actions { input MP }
        }

        def NODE "Sweep" {
            chance 15<percent>
            actions { input D HK }
        }

        def NODE "QuickAttack" { actions { input LP } }

        def NODE "AntiAir" {
            stance Grounded
            condition (opponent (Var.Entity.isAirborne .& !Var.Entity.inStunLike))

            actions {
                log "AI: Doing anti-air"
                input DP HP
            }
        }

        def NODE "Fireball" {
            decorator { log "fireball tick..." }

            actions {
                inc "counter"
                log "Doing Fireball" &&"counter"
                wait 12<frames>
                input QCF MP
            }
        }

        def NODE "DonkeyKick.EX" {
            condition (res r.Energy .>= 100)
            actions { input !B F KK }
        }

        def NODE "Tatsu" { actions { input HCB LK } }

        def NODE "Advance" { actions { input F 5<times> } }

        def NODE "DiveKick" {
            stance Airborne
            actions { input QCB HK }
        }

        def NODE "Approach" {
            neutralOnly
            distance 200

            nodes {
                chance 16<percent> "JumpIn"
                chance 25<percent> "DashForward"
                "WalkForward"
            }
        }

        def NODE "JumpIn" {
            neutralOnly
            weighted

            actions {
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

            actions {
                wait 12<frames>
                input HK
            }
        }

        def NODE "Turtle" {
            chance 20<percent>
            actions { input DB 16<times> }
        }

        def NODE "WalkForward" { actions { input F 8<times> } }
        def NODE "WalkBack" { actions { input B 8<times> } }
        def NODE "DashForward" { actions { input F F } }
    }
