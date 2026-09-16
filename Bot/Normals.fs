module PrototypeFighter.Scripts.Bot.Normals

open FGScript
open PrototypeFighter.Scripts

let states =
    def PARTIAL {
        def STATE s.St.LP {
            stance Stand
            category Normal

            trigger { command LP }

            transitions {
                chain t.allSpecials

                transitions {
                    afterFrame 8<frames>
                    chain s.St.LP
                    chain s.Cr.LP
                }
            }

            attack {
                light
                strike
                punch
                looks High
                set Stun.Light
                damage 30
                gain r.Energy 10
                set JuggleLimit p.JL.Normals
                pushbackLinear
                set HitFlags.Flipout
            }

            action {
                phase Startup
                playVoice v.Attack.Light Skippable
                animate a.St.LP 0 2

                phase Active
                playSound Sfx.Swing.LP
                animate a.St.LP 3 5

                phase Recovery
                animate a.St.LP 6 12
            }
        }

        def STATE s.St.MP {
            stance Stand
            category Normal
            trigger { command MP }

            transitions {
                chain t.allSpecials 16<window>

                chainTo s.Chain.MP_HP {
                    lenience 10<frames>
                    cancelWindow 30<frames>
                    before 9<frames>
                }
            }

            attack {
                medium
                strike
                punch
                set HitStun 22<frames>
                set BlockStun 21<frames>
                set HitStop 12<frames>
                damage 60
                damage r.Stamina 8 OnBlock
                gain r.Energy 20
                set JuggleLimit p.JL.Normals
                pushback OnHit 33
                pushbackFriction 0<percent>
            }

            action {
                phase Startup
                playVoice v.Attack.Medium Skippable
                // freeze 30<frames>
                animate a.St.MP 0 4

                phase Active
                playSound Sfx.Swing.MP
                animate a.St.MP 5 8

                phase Recovery
                animate a.St.MP 9 19
            }
        }

        def STATE s.St.HP {
            stance Stand
            category Normal

            trigger { command HP }

            attack {
                heavy
                strike
                punch
                damage 80
                damage r.Stamina 20 OnBlock
                gain r.Energy 30
                looks High
                set Stun.Level[6]
                set BlockStun 15<frames>
                set JuggleLimit p.JL.Normals
                set (On(CounterHit, Punish)) HitFlags.Stagger
                freeze (On(CounterHit, Punish)) 8<frames>
                freezeSlowMotion
            }

            let chargeBend = var ()
            let chargeTime = 20

            action {
                On DidHit { shake 15<frames> (7, 9) }
                disable ArmorBox
                phase Startup
                animate a.St.HP 1 5

                If(
                    Query.Input {
                        command HP
                        hold 5<frames>
                    }
                ) {
                    set chargeBend 0
                    enable ArmorBox
                    On GotArmorHit { log "ARMOR: Absorbed attack" }

                    While(Query.Input { command HP } .& (Var.currentFrame .<= chargeTime)) {
                        pose a.St.HP 5
                        rotateBone bones.Spine Axis.Y chargeBend
                        sub chargeBend 2
                        frame
                    }
                }

                If(Var.currentFrame .> chargeTime) {
                    log "HP: Fully Charged"
                    add BlockStun 9<frames>
                    add HitStun 7<frames>
                    attackFreeze 13<frames> (On(CounterHit, Punish))

                    hitFlags (On Punish) HitFlags.CausesCrumple
                    hitSound Sfx.Hit.Power (On Punish)
                    hitSpark Vfx.HitSpark.PunishCrumple (On Punish)
                }

                playVoice v.Attack.Heavy Skippable
                move Forward 10
                frame a.St.HP 6
                move Forward 10
                frame a.St.HP 7
                move Forward 10
                frame a.St.HP 8
                move Forward 10
                frame a.St.HP 9
                move Forward 10

                phase Active
                playSound Sfx.Swing.HP
                animate a.St.HP 10 14

                phase Recovery
                animate a.St.HP 15 31
                frame a.St.HP 31
            }
        }

        def STATE s.St.LK {
            stance Stand
            category Normal

            trigger { command LK }

            transitions { chain t.allSpecials }

            attack {
                light
                strike
                kick
                looks Low
                damage 30
                gain r.Energy 10
                set Stun.Light
                set JuggleLimit p.JL.Normals
                set HitFlags.Flipout
            }

            action {
                phase Startup
                playVoice v.Attack.Light Skippable
                playSound Sfx.Swing.LK
                animate a.St.LK 0 3

                phase Active
                animate a.St.LK 4 6

                phase Recovery
                animate a.St.LK 7 17
            }
        }

        def STATE s.St.MK {
            stance Stand
            category Normal

            trigger { command MK }

            transitions {
                chainTo s.St.MK {
                    condition (Query.StateComboOccurrences(s.St.MK) == 1)
                    call "ApplyMinDamageScaling" (args 30)
                    cancelWindow 36<frames>
                    lenience 16<frames>
                    after 20<frames>
                    skip 6<frames>
                }

                chainTo s.Chain.MP_HP_HK {
                    condition (Query.StateComboOccurrences(s.St.MK) == 2)
                    cancelWindow 30<frames>
                    lenience 12<frames>
                }
            }

            attack {
                medium
                strike
                kick
                damage 60
                damage r.Stamina 15 OnBlock
                gain r.Energy 15
                set Stun.Level[4]
                set JuggleLimit p.JL.Normals
            }

            action {
                phase Startup
                playVoice v.Attack.Medium Skippable
                animate a.St.MK 0 8

                phase Active
                playSound Sfx.Swing.MK
                animate a.St.MK 9 11

                phase Recovery
                animate a.St.MK 12 28
            }
        }

        def STATE s.St.HK {
            stance Stand
            category Normal

            trigger { command HK }

            attack {
                heavy
                strike
                kick
                looks High
                damage 90
                damage r.Stamina 20 OnBlock
                gain r.Energy 40
                set HitStun 28<frames>
                set BlockStun 14<frames>
                set HitStop 13<frames>
                set JuggleLimit p.JL.Normals
                pushback OnBlock 25
                pushback OnHit 40
                pushbackFriction 80<percent>
                set (On Punish) HitFlags.CausesCrumple
                hitSound (On Punish) Sfx.Hit.Power
                hitSpark (On Punish) Vfx.HitSpark.PunishCrumple
                freeze (On CounterHit) 10<frames>
                freeze (On Punish) 30<frames>
                freezeSlowMotion
            }

            action {
                // Ping test
                send Signal.Opponent (num 1) Var.currentRoundFrame

                On DidHit { shake 20<frames> (10, 12) }

                phase Startup
                playVoice v.Attack.Heavy Skippable
                animate a.St.HK 0 11

                phase Active
                playSound Sfx.Swing.HK
                animate a.St.HK 12 15

                phase Recovery
                animate a.St.HK 16 34
            }
        }

        def STATE s.Cr.LP {
            stance Crouch
            category Normal
            priority 1

            trigger {
                command D LP
                strict false
            }

            transitions {
                chain t.allSpecials

                transitions {
                    afterFrame 8<frames>
                    chain s.Cr.LK
                    chain s.Cr.LP
                    chain s.St.LP
                }
            }

            attack {
                light
                strike
                punch
                damage 30
                gain r.Energy 10
                set Stun.Light
                set JuggleLimit p.JL.Normals
                pushbackLinear
                set HitFlags.Flipout
            }

            action {
                phase Startup
                animate a.Cr.LP 0 2

                phase Active
                playSound Sfx.Swing.LP
                animate a.Cr.LP 3 4

                phase Recovery
                animate a.Cr.LP 5 13
            }
        }

        def STATE s.Cr.MP {
            stance Crouch
            category Normal
            priority 1

            trigger {
                command D MP
                strict false
            }

            transitions {
                chain t.allSpecials

                chainTo s.Chain.cMP_cHP {
                    lenience 10<frames>
                    cancelWindow 30<frames>
                    before 9<frames>
                }
            }

            attack {
                medium
                strike
                punch
                damage 60
                damage r.Stamina 8 OnBlock
                gain r.Energy 20
                set Stun.Medium
                set JuggleLimit p.JL.Normals
                initialScaling 95<percent>
            }

            action {
                phase Startup
                playVoice v.Attack.Medium Skippable
                animate a.Cr.MP 0 4

                phase Active
                playSound Sfx.Swing.MP
                animate a.Cr.MP 5 8

                phase Recovery
                animate a.Cr.MP 9 21
            }
        }

        def STATE s.Cr.HP {
            stance Crouch
            category Normal
            priority 1

            trigger {
                command D HP
                strict false
            }

            transitions {
                chain t.allSpecials

                chainTo t.allJumps {
                    On(CounterHit)
                    call "StaminaCancelBlink"
                }
            }

            attack {
                heavy
                strike
                punch
                damage 80
                damage r.Stamina 20 OnBlock
                gain r.Energy 40
                set Stun.Level[6]
                extra HitStun 2<frames>
                set JuggleLimit p.JL.Normals
                set KnockUp (On Punish) (1.5, 15.0) p.FallGravity
                set KnockUp (On CounterHit) (1.5, 8.5) p.FallGravity
            }

            action {
                phase Startup
                playVoice v.Attack.Heavy Skippable
                animate a.Cr.HP 0 7

                phase Active
                playSound Sfx.Swing.HP
                animate a.Cr.HP 8 13

                phase Recovery
                animate a.Cr.HP 14 34
            }
        }

        def STATE s.Cr.LK {
            stance Crouch
            category Normal
            priority 1

            trigger {
                command D LK
                strict false
            }

            transitions {
                chain s.Cr.LP
                chain s.St.LP
                empty 15<frames> 4<skip> s.Cr.LK
            }

            attack {
                light
                strike
                kick
                guard Low
                damage 20
                gain r.Energy 10
                set Stun.Light
                set JuggleLimit p.JL.Normals
                set HitFlags.Flipout
            }

            action {
                phase Startup
                playVoice v.Attack.Light Skippable
                animate a.Cr.LK 1 4

                phase Active
                playSound Sfx.Swing.LK
                animate a.Cr.LK 5 6

                phase Recovery
                animate a.Cr.LK 7 16
            }
        }

        def STATE s.Cr.MK {
            stance Crouch
            category Normal
            priority 1

            trigger {
                command D MK
                strict false
            }

            transitions {
                cancelWindow 26<frames>
                chain t.allSpecials
            }

            attack {
                medium
                strike
                kick
                guard Low
                damage 50
                damage r.Stamina 5 OnBlock
                gain r.Energy 15
                set Stun.Level[4]
                set JuggleLimit p.JL.Normals
                set HitFlags.Flipout
            }

            action {
                phase Startup
                playVoice v.Attack.Medium Skippable
                animate a.Cr.MK 0 6

                phase Active
                playSound Sfx.Swing.MK
                animate a.Cr.MK 7 9

                phase Recovery
                animate a.Cr.MK 10 28
            }
        }

        def STATE s.Cr.HK {
            stance Crouch
            category Normal
            priority 1

            trigger {
                command D HK
                strict false
            }

            attack {
                heavy
                strike
                kick
                guard Low
                damage 90
                damage r.Stamina 20 OnBlock
                gain r.Energy 30
                set Stun.Heavy
                set BlockStun 14<frames>
                set JuggleLimit p.JL.Normals
                pushback OnBlock 25
                pushback OnHit 50
                downTime 10<frames>
                set HitFlags.Sweep
                set (On(Punish, CounterHit)) HitFlags.HardKnockDown
            }

            action {
                phase Startup
                playVoice v.Attack.Heavy Skippable
                animate a.Cr.HK 0 7

                phase Active
                playSound Sfx.Swing.HK
                animate a.Cr.HK 8 10

                phase Recovery
                animate a.Cr.HK 11 33
            }
        }

        def STATE s.Jump.LP {
            stance Airborne
            category Normal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command LP
            }

            attack {
                light
                punch
                damage 30
                gain r.Energy 10
                set Stun.Light
                set JuggleLimit p.JL.AirNormals
                set HitFlags.Flipout
            }

            action {
                On Landing { exit }
                phase Startup
                playVoice v.Attack.Light Skippable
                animate a.Jump.LP 1 4

                phase Active
                playSound Sfx.Swing.LP
                frame a.Jump.LP 5 10<times>
                phase Recovery
                animate a.Jump.LP 6 25
                Loop { frame }
            }
        }

        def STATE s.Jump.MP {
            stance Airborne
            category Normal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command MP
            }

            transitions { chain t.airborneSpecials }

            attack {
                medium
                punch
                damage 35
                gain r.Energy 10
                set Stun.Medium
                set JuggleLimit 8
                set Air KnockBack (3.5, 8.5) p.FallGravity
            }

            action {
                On Landing { exit }

                phase Startup
                playVoice v.Attack.Medium Skippable
                animate a.Jump.MP 0 6

                phase Active
                playSound Sfx.Swing.MK
                animate a.Jump.MP 7 9
                hitAgain
                inc JuggleLimit
                frame a.Jump.MP 9 2<times>

                phase Recovery
                animate a.Jump.MP 10 29
                Loop { frame }
            }
        }

        def STATE s.Jump.HP {
            stance Airborne
            category Normal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command HP
            }

            attack {
                heavy
                punch
                damage 80
                damage r.Stamina 10 OnBlock
                gain r.Energy 35
                set Stun.Heavy
                set JuggleLimit p.JL.Normals
                set (On(CounterHit, Punish)) HitFlags.SpikeDown
            }

            action {
                On Landing { exit }
                phase Startup
                playVoice v.Attack.Heavy Skippable
                animate a.Jump.HP 0 7

                phase Active
                playSound Sfx.Swing.HP
                frame a.Jump.HP 8 6<times>
                phase Recovery
                animate a.Jump.HP 9 28
                Loop { frame }
            }
        }

        def STATE s.Jump.LK {
            stance Airborne
            category Normal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command LK
            }

            attack {
                light
                kick
                damage 30
                gain r.Energy 10
                set Stun.Light
                set JuggleLimit p.JL.Normals
                set HitFlags.Flipout
            }

            action {
                On Landing { exit }
                phase Startup
                playVoice v.Attack.Light Skippable
                animate a.Jump.LK 0 4

                phase Active
                playSound Sfx.Swing.MK
                frame a.Jump.LK 5 10<times>
                phase Recovery
                animate a.Jump.LK 6 25
                Loop { frame }
            }
        }

        def STATE s.Jump.MK {
            stance Airborne
            category Normal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command MK
            }

            transitions { chain s.Chain.jMK_jHK }

            attack {
                medium
                kick
                damage 50
                gain r.Energy 20
                set Stun.Medium
                set JuggleLimit p.JL.Normals
            }

            action {
                On Landing { exit }
                phase Startup
                playVoice v.Attack.Medium Skippable
                animate a.Jump.MK 0 5

                phase Active
                playSound Sfx.Swing.MK
                frame a.Jump.MK 6 6<times>
                phase Recovery
                animate a.Jump.MK 7 26
                Loop { frame }
            }
        }


        def STATE s.Jump.HK {
            stance Airborne
            category Normal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command HK
            }

            attack {
                heavy
                kick
                damage 80
                damage r.Stamina 15 OnBlock
                gain r.Energy 40
                set Stun.Heavy
                set JuggleLimit p.JL.Normals
                set Air KnockUp (On(CounterHit, Punish)) (4.5, 10.0) p.Gravity
                set HitFlags.Flipout
            }

            action {
                On Landing { exit }
                phase Startup
                playVoice v.Attack.Heavy Skippable
                animate a.Jump.HK 0 8

                phase Active
                playSound Sfx.Swing.HK
                frame a.Jump.HK 9 8<times>
                phase Recovery
                animate a.Jump.HK 10 29
                Loop { frame }
            }
        }

        def STATE s.St.F_MP {
            stance Stand
            category CommandNormal
            priority 1

            trigger { command F MP }

            attack {
                medium
                punch
                guard High
                damage 15
                gain r.Energy 10
                pushback OnBlock 8
                pushback OnHit 20
                set HitStun 20<frames>
                set BlockStun 14<frames>
                set HitStop 11<frames>
                set JuggleLimit 1
                set HitFlags.Flipout
            }

            action {
                phase Startup
                animate a.St.F_MP 1 14
                playVoice v.Attack.Medium
                animate a.St.F_MP 15 19

                phase Active
                playSound Sfx.Swing.MP
                animate a.St.F_MP 20 22
                hitAgain
                frame a.St.F_MP 23

                phase Recovery
                animate a.St.F_MP 25 42
            }
        }

        def STATE s.St.B_MK {
            stance Stand
            category CommandNormal
            priority 1

            trigger { command B MK }

            attack {
                heavy
                kick
                damage 40
                gain r.Energy 20
                set Stun.Medium
                set KnockUp (1.5, 14.0) p.FallGravity
                set Air KnockUp (1.5, 12.0) p.FallGravity
                set JuggleStart 2
                set JuggleIncrease 3
                set JuggleLimit 10
                pushback OnHit 60
                pushbackLinear
                addScaling 10<percent>
            }

            action {
                phase Startup
                playVoice v.Attack.Medium Skippable
                animate a.St.B_MK 1 9

                phase Active
                playSound Sfx.Swing.MK
                animate a.St.B_MK 10 15

                phase Recovery
                animate a.St.B_MK 16 35
            }
        }

        def STATE s.Chain.MP_HP {
            stance Stand
            category Normal

            trigger {
                command HP
                inputWindow 16<frames>
            }

            transitions { chain s.Chain.MP_HP_HK 30<window> 10<lenience> }

            attack {
                heavy
                strike
                punch
                looks High
                damage 60
                damage r.Stamina 10 OnBlock
                gain r.Energy 25
                set Stun.Heavy
                maxScaling 95<percent>
                pushbackFriction 90<percent>
            }

            useAction s.St.HP
        }

        def STATE s.Chain.MP_HP_HK {
            stance Stand
            category CommandNormal

            trigger {
                command HK
                inputWindow 16<frames>
            }

            transitions { t.specialDashCancellable }

            attack {
                heavy
                strike
                kick
                looks High
                damage 90
                damage r.Stamina 15 OnBlock
                gain r.Energy 30
                set Stun.Heavy
                set KnockBack (5.4, 8.0) p.KnockGravity
            }

            useAction s.St.HK
        }

        def STATE s.Chain.cMP_cHP {
            stance Stand
            category CommandNormal
            priority 1

            trigger {
                command D HP
                strict false
                inputWindow 16<frames>
            }

            transitions {
                cancelWindow 24<frames>
                chain t.allSpecials
                chain t.allJumps
            }

            attack {
                heavy
                strike
                punch
                damage 50
                damage r.Stamina 10 OnBlock
                gain r.Energy 20
                set Stun.Heavy
                set JuggleStart 2
                set JuggleLimit 10
                set KnockUp (1.5, 13.0) p.FallGravity
                pushback OnHit 50
            }

            useAction s.Cr.HP
        }

        def STATE s.Chain.jMK_jHK {
            stance Airborne
            category CommandNormal
            keepFacingSide
            next s.Jump.Landing

            trigger {
                strict false
                command HK
            }

            attack {
                heavy
                kick
                damage 80
                gain r.Energy 20
                set Stun.Heavy
                set HitFlags.SpikeDown
            }

            useAction s.Jump.HK
        }
    }
