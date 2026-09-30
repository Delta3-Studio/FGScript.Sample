module PrototypeFighter.Scripts.Bot.Base

open FGScript
open PrototypeFighter.Scripts
open Macros

let states =
    def PARTIAL {
        def STATE s.Neutral {
            stance Stand
            category Neutral

            transitions {
                s.Cr.Neutral
                s.Walk.F
                s.Walk.B
                t.allDashes
                t.allJumps
                t.regularThrows
                t.allSpecials
                t.allNormals
                t.allTaunts
            }

            let frameCount = 166
            let loop = "loop"

            action {
                On ActionBegin { call "CheckThrowInvuln" }
                On EachFrame { gain r.Stamina p.StaminaRecovery.Normal }
                On ChangeSide { gotoParent "turned" true }

                If(Query.LastState s.Cr.Neutral) {
                    animate Blend (a.Cr.Idle, a.St.Idle) 0 5
                    animate a.St.Idle 6 frameCount
                    goto loop
                }

                If(Query.LastStance Crouch) {
                    animate Blend (a.Cr.Idle, a.St.Idle) 0 5
                    animate a.St.Idle 6 frameCount
                    goto loop
                }

                If(Query.LastState s.Walk.F) {
                    animate Blend (a.Walk.F, a.St.Idle) 0 5
                    animate a.St.Idle 6 frameCount
                    goto loop
                }

                If(Query.LastState s.Walk.B) {
                    animate Blend (a.Walk.B, a.St.Idle) 0 5
                    animate a.St.Idle 6 frameCount
                    goto loop
                }

                If(Query.LastState s.Jump.Landing) {
                    animate a.Jump.Landing 4 20
                    goto loop
                }

                If(Query.LastState s.Dash.F) {
                    animate a.Dash.F 19 24
                    goto loop
                }

                If(Query.LastState s.Dash.B) {
                    animate a.Dash.B 23 30
                    goto loop
                }

                If(Query.LastState s.WakeUp.FaceUp) {
                    animate a.WakeUp.FaceUp 31 42
                    goto loop
                }

                If(Query.LastState s.WakeUp.FaceDown) {
                    animate a.WakeUp.FaceDown 31 42
                    goto loop
                }

                If(Query.LastCategory(Guard, Blockstun)) {
                    animate a.St.Guard 34 44
                    goto loop
                }

                goto loop
                label "turned"
                animate a.St.Turn 0 7
                goto loop

                label loop
                Loop { animate a.St.Idle 0 frameCount }
            }
        }

        def STATE s.Cr.Neutral {
            stance Crouch
            category Neutral

            trigger {
                skipRelease

                command D
                command DB
                command DF
            }

            transitions {
                t.regularThrows
                t.allJumps
                t.allNormals
                t.allSpecials
                t.allTaunts
            }

            action {
                On ActionBegin { call "CheckThrowInvuln" }

                On EachFrame {
                    gain r.Stamina p.StaminaRecovery.Normal
                    Unless(Query.Input { command D }) { exit }
                }

                On ChangeSide { gotoParent "turned" true }

                If(Query.LastState(s.Cr.Guard, s.Cr.BlockStun)) {
                    animate a.Cr.Guard 34 44
                    goto "loop"
                }

                If(Query.LastState s.WakeUp.FaceUp) {
                    animate Blend (a.WakeUp.FaceUp, 31) a.Cr.Idle 0 8
                    goto "loop"
                }

                If(Query.LastState s.WakeUp.FaceDown) {
                    animate Blend (a.WakeUp.FaceDown, 31) a.Cr.Idle 0 8
                    goto "loop"
                }

                Unless(Query.LastStance Crouch) {
                    animate Blend (a.St.Idle, a.Cr.Idle) 0 8
                    animate a.Cr.Idle 9 100
                    goto "loop"
                }

                goto "loop"
                label "turned"
                animate a.Cr.Turn 0 7
                goto "loop"

                label "loop"
                Loop { animate a.Cr.Idle 0 100 }
            }
        }

        def STATE s.Fall {
            stance Airborne
            category Neutral
            next s.Jump.Landing

            action {
                set Gravity p.Gravity
                On Landing { exit }

                If(var Position.Y .> 60) { animate a.Jump.Neutral.Air 1 40 }
                Loop { frame }
            }
        }

        def STATE s.St.Guard {
            stance Stand
            category Guard

            trigger {
                command B
                skipRelease
            }

            transitions {
                t.allJumps
                t.regularThrows
                t.allSpecials
                t.allNormals
                t.allTaunts
            }

            action {
                On EachFrame { If !(Var.Entity.inProximityGuard .& (Query.Input { B })) { exit } }
                animate a.St.Guard 1 11
                Loop { animate a.St.Guard 12 33 }
            }
        }

        def STATE s.Cr.Guard {
            stance Crouch
            category Guard

            trigger {
                command DB
                skipRelease
            }

            transitions {
                t.allJumps
                t.regularThrows
                t.allSpecials
                t.allNormals
                t.allTaunts
            }

            action {
                On EachFrame { If(!(Var.Entity.inProximityGuard .& (Query.Input { DB }))) { exit } }
                animate a.Cr.Guard 1 11
                Loop { animate a.Cr.Guard 12 33 }
            }
        }

        def STATE s.PushBlock {
            stance Stand
            category Special
            cost r.Stamina 150

            trigger { command F (HP + HK) }

            attack {
                strike
                damage 0
                force Hit
                pushback OnHit 60
                pushback OnBlock 25
                pushbackTime OnHit 12<frames>
                pushbackFriction 100<percent>
                set BlockStun 20<frames>
                set HitStop 9<frames>
                armorBreak
                set KnockBack (6, 8) 0.8
                set HitFlags.SoftKnockDown
            }

            action {
                On ActionBegin {
                    enable FullyInvuln 20<frames>
                    enable BoxLayer.Layer2
                    playSound Sfx.Blink
                    playSound Sfx.ActivateEX
                    effect Vfx.Blink Target.Self [ Translate(Axis.Y, 80) ]
                }

                phase Startup
                frame a.Sp.Fireball 1
                freeze 5<frames>
                move Forward 10
                fitAnimate (num 10) a.Sp.Fireball 5 29
                move Forward 10
                frame a.Sp.Fireball 30
                move Forward 10

                phase Active
                animate a.Sp.Fireball 31 33

                phase Recovery
                fitAnimate (num 37) a.Sp.Fireball 45 65
            }
        }

        def STATE s.Anim.Outro.Victory {
            stance Stand
            category Outro
            require Grounded
            condition (Args.Outro.result === RoundResult.Win)

            action {
                On ActionBegin { stop }
                animate a.Outro.Victory 0 30
                playVoice v.Outro.Vitory (Pitch 2<percent>)
                animate a.Outro.Victory 31 80
                frame a.Outro.Victory 80 10<times>
            }
        }

        def STATE s.Anim.Outro.Timeout {
            stance Stand
            category Outro
            require Grounded
            condition (Args.Outro.result === RoundResult.Timeout)

            action {
                On ActionBegin { stop }
                animate a.Outro.Timeout 0 10
                playVoice v.Outro.Annoyed (Pitch 2<percent>)
                animate a.Outro.Timeout 11 90
                frame a.Outro.Timeout 90 30<times>
            }
        }

        def STATE s.Anim.Outro.DieStand {
            stance Stand
            category Outro
            require Grounded
            condition (Args.Outro.result === RoundResult.Loose)

            action {
                On ActionBegin {
                    stop
                    disable HurtBox GrabBox
                }

                animate Blend (a.St.Idle, 1) a.HitStun.Crumple 1 20
                animate a.HitStun.Crumple 20 55
                effect Vfx.Dust Floor [ Translate(Axis.X, -40); Rotate(Axis.Z, -15.<deg>) ]
                effect Vfx.Dust Floor [ Translate(Axis.X, 60); Rotate(Axis.Z, -165.<deg>) ]
                playSound Sfx.KnockDown.Soft (Volume -10<Db>)
                animate a.HitStun.Crumple 56 63
                frame a.LyingDown.FaceDown 0 30<times>
            }
        }

        def STATE s.Taunt.Hurry {
            stance Stand
            category Taunt
            priority 1
            require Grounded

            trigger {
                command D D (HP + HK)
                negativeEdge false
                motionGap 5<frames>
            }

            action {
                animate a.Taunt.Hurry 0 30
                playVoice v.Outro.Taunt
                animate a.Taunt.Hurry 31 100
                gain r.Energy 100
                animate a.Taunt.Hurry 101 120
            }
        }

        def FUNC "CheckThrowInvuln" {
            If(Query.LastCategory WakeUp) { enable ThrowInvuln 1<frames> }
            ElseIf(Query.LastCategory Blockstun) { enable ThrowInvuln 2<frames> }
        }

        def FUNC "StaminaCancelBlink" {
            aura Vfx.Auras.White 6<frames>
            playSound Sfx.Blink
            gainCooldown r.Stamina 60<frames>
            attackAddDamageScaling 5<percent>
        }

        def FUNC "SpecialCancelBlink" {
            aura Vfx.Auras.White 9<frames>
            playSound Sfx.Blink
            effect Vfx.Blink At.Center
            gainCooldown r.Stamina 150<frames>
            attackAddDamageScaling 10<percent>
        }

        def FUNC "ApplyMinDamageScaling" { attackMinDamageScaling Arg1 }
    }
