module PrototypeFighter.Scripts.Bot.Movement

open FGScript
open PrototypeFighter.Scripts

let states =
    def PARTIAL {
        def STATE s.Walk.F {
            stance Stand
            category Movement
            validateNeutral

            trigger {
                command F
                lenience 0<frames>
                skipRelease
            }

            transitions {
                t.allAttacks
                t.allJumps
                t.allDashes
                t.regularThrows
                s.Cr.Neutral
                s.Walk.B
            }

            action {
                On ActionEnd { set Vel.X 0 }

                On EachFrame {
                    gain r.Stamina p.StaminaRecovery.Fast
                    Unless(Query.Input { F }) { exit }
                }

                // the first frame has 25% of the horizontal speed
                set Vel.X (25 %= p.WalkSpeed)

                Timer 1<frames> {
                    boundState
                    set Vel.X p.WalkSpeed
                }

                If(Query.LastCategory(Guard, Blockstun)) {
                    animate Blend (a.St.Guard, 34) a.Walk.F 0 10
                    goto "walk"
                }

                If(Query.LastState s.WakeUp.FaceUp) {
                    animate Blend (a.WakeUp.FaceUp, 31) a.Walk.F 0 10
                    goto "walk"
                }

                If(Query.LastState s.WakeUp.FaceDown) {
                    animate Blend (a.WakeUp.FaceDown, 31) a.Walk.F 0 10
                    goto "walk"
                }

                animate Blend (a.St.Idle, a.Walk.F) 0 10

                label "walk"
                animate a.Walk.F 11 17
                playSound Sfx.FootStep
                animate a.Walk.F 18 46
                playSound Sfx.FootStep
                animate a.Walk.F 47 61

                Loop {
                    animate a.Walk.F 0 17
                    playSound Sfx.FootStep
                    animate a.Walk.F 18 46
                    playSound Sfx.FootStep
                    animate a.Walk.F 47 61
                }
            }
        }

        def STATE s.Walk.B {
            stance Stand
            category Movement
            allow Meta.Guard.All
            validateNeutral

            trigger {
                command B
                lenience 0<frames>
                skipRelease
            }

            transitions {
                t.allAttacks
                t.allJumps
                t.allDashes
                t.regularThrows
                s.Cr.Neutral
                s.Walk.F
            // except s.St.HK // just for testing
            }

            action {
                On ActionEnd { set Vel.X 0 }

                On EachFrame {
                    gain r.Stamina p.StaminaRecovery.Slow
                    Unless(Query.Input { B }) { exit }
                }

                // the first frame has 25% of the horizontal speed
                set Vel.X (25 %= p.BackWalkSpeed)

                Timer 1<frames> {
                    boundState
                    set Vel.X p.BackWalkSpeed
                }

                If(Query.LastCategory(Guard, Blockstun)) {
                    animate Blend (a.St.Guard, 34) a.Walk.B 0 10
                    goto "walk"
                }

                If(Query.LastState s.WakeUp.FaceUp) {
                    animate Blend (a.WakeUp.FaceUp, 31) a.Walk.B 0 10
                    goto "walk"
                }

                If(Query.LastState s.WakeUp.FaceDown) {
                    animate Blend (a.WakeUp.FaceDown, 31) a.Walk.B 0 10
                    goto "walk"
                }

                animate Blend (a.St.Idle, a.Walk.B) 0 10

                label "walk"
                animate a.Walk.B 11 19
                playSound Sfx.FootStep
                animate a.Walk.B 20 37
                playSound Sfx.FootStep (Volume -8<Db>)

                Loop {
                    animate a.Walk.B 0 19
                    playSound Sfx.FootStep
                    animate a.Walk.B 20 37
                    playSound Sfx.FootStep (Volume -8<Db>)
                }
            }
        }

        let karaFrames = 4<frames>

        def STATE s.Jump.Neutral {
            stance Airborne
            category Movement
            require Grounded

            trigger {
                command U
                lenience 0<frames>
                skipRelease
                condition (FinalPosition.Y == 0)
            }

            transitions {
                beforeFrame karaFrames
                kara t.allSpecials
                kara s.Jump.Forward
                kara s.Jump.Backward
                empty karaFrames t.airNormals
                t.airborneSpecials
            }

            next s.Jump.Landing

            action {
                On EachFrame { gain r.Stamina p.StaminaRecovery.Normal }

                On ActionBegin {
                    stance Stand
                    enable ThrowInvuln 4<frames>
                }
                // 4 pre-jump frames
                animate a.Jump.Neutral.Start 0 3
                stance Default

                playSound Sfx.Jump
                playVoice v.Jump (PlayRate 90<percent>)
                effect Vfx.Dust [ Rotate(Axis.Z, -90.<deg>); Scale(Axis.XYZ, 1.5) ]

                set Gravity p.Gravity
                set Vel.Y p.Jump.VerticalSpeed

                On ActionEnd { log "Jump Neutral Ended!" }

                On Landing {
                    log "Landed!"
                    exit
                }

                animate a.Jump.Neutral.Air 1 40
                Loop { frame }
            }
        }

        def STATE s.Jump.Forward {
            stance Airborne
            category Movement
            require Grounded

            trigger {
                command UF
                lenience 0<frames>
                skipRelease
                condition (var Position.Y == 0)
            }

            transitions {
                kara karaFrames t.allSpecials
                empty karaFrames t.airNormals
                t.airborneSpecials
            }

            next s.Jump.Landing

            action {
                On EachFrame { gain r.Stamina p.StaminaRecovery.Normal }

                On ActionBegin {
                    stance Stand
                    enable ThrowInvuln 4<frames>
                }
                // 4 pre-jump frames
                animate a.Jump.Forward.Start 0 3
                stance Default

                effect Vfx.Dust [ Rotate(Axis.Z, -105.<deg>); Scale(Axis.XYZ, 1.5) ]
                playSound Sfx.Jump
                playVoice v.Jump (PlayRate 90<percent>)

                set Gravity p.Gravity
                set Vel.Y p.Jump.VerticalSpeed
                set Vel.X p.Jump.ForwardSpeed

                On Landing { exit }

                animate a.Jump.Forward.Air 1 40
                Loop { frame }
            }
        }

        def STATE s.Jump.Backward {
            stance Airborne
            category Movement
            require Grounded

            trigger {
                command UB
                lenience 0<frames>
                skipRelease
                condition (var Position.Y == 0)
            }

            transitions {
                kara karaFrames t.allSpecials
                empty karaFrames t.airNormals
            }

            next s.Jump.Landing

            action {
                On EachFrame { gain r.Stamina p.StaminaRecovery.Slow }

                On ActionBegin {
                    stance Stand
                    enable ThrowInvuln 4<frames>
                }
                // 4 pre-jump frames
                animate a.Jump.Backward.Start 0 3
                stance Default

                effect Vfx.Dust [ Rotate(Axis.Z, -75.<deg>); Scale(Axis.XYZ, 1.5) ]
                playSound Sfx.Jump
                playVoice v.Jump (PlayRate 90<percent>)

                set Gravity p.Gravity
                set Vel.Y p.Jump.VerticalSpeed
                set Vel.X p.Jump.BackwardSpeed

                On Landing { exit }

                animate a.Jump.Backward.Air 1 40
                Loop { frame }
            }
        }

        let cancelableStart = var ()

        def STATE s.Jump.Landing {
            stance Stand
            category Movement
            allow Meta.Block.All When cancelableStart

            transitions {
                empty 1<frames> t.allAttacks
                empty 1<frames> s.Cr.Neutral When cancelableStart
            }

            action {
                On ActionBegin {
                    clear Vel
                    clear Gravity

                    If(Query.LastCategory Hitstun) {
                        set cancelableStart
                        disable HurtBox
                        pass
                    }

                    set cancelableStart (Query.LastState t.allJumps)
                    Unless(cancelableStart) { phase Recovery }
                }

                call "LandEffect"

                // 3 landing recovery frames
                animate a.Jump.Landing 1 3
            }
        }

        def FUNC "LandEffect" {
            playSound Sfx.Landing
            effect Vfx.Dust Floor [ Translate(Axis.X, -30); Scale(Axis.XYZ, 0.8); Rotate(Axis.Z, -15.<deg>) ]
            effect Vfx.Dust Floor [ Translate(Axis.X, 30); Scale(Axis.XYZ, 0.8); Rotate(Axis.Z, -165.<deg>) ]
        }

        def STATE s.Dash.F {
            stance Stand
            category Movement

            trigger { command F F }

            transitions { kara karaFrames t.allSpecials }

            action {
                On EachFrame { gain r.Stamina p.StaminaRecovery.Fast }

                On ActionEnd {
                    set Vel.X 0
                    set Acc.X 0
                }

                set Vel.X (20 %= p.DashSpeed)
                animate a.Dash.F 0 2

                set Vel.X (40 %= p.DashSpeed)
                animate a.Dash.F 3 5

                playSound Sfx.Dash
                effect Vfx.Dust At.Foot

                set Vel.X p.DashSpeed
                animate a.Dash.F 6 10

                set Acc.X (5 %= -p.DashSpeed)
                animate a.Dash.F 11 18
            }
        }

        def STATE s.Dash.B {
            stance Stand
            category Movement

            trigger { command B B }

            transitions { kara karaFrames t.allSpecials }

            action {
                On EachFrame { gain r.Stamina p.StaminaRecovery.Slow }

                On ActionEnd {
                    set Vel.X 0
                    set Acc.X 0
                }

                set Vel.X (20 %= p.BackdashSpeed)
                frame a.Dash.B 0
                frame a.Dash.B 1

                set Vel.X (40 %= p.BackdashSpeed)
                animate a.Dash.B 2 5

                playSound Sfx.Dash
                effect Vfx.Dust At.Foot [ Flip Axis.X ]

                set Vel.X p.BackdashSpeed
                animate a.Dash.B 6 10

                set Acc.X (5 %= -p.BackdashSpeed)
                animate a.Dash.B 12 22
            }
        }
    }
