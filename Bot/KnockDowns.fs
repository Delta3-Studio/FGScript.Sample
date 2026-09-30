module PrototypeFighter.Scripts.Bot.KnockDowns

open FGScript
open PrototypeFighter.Scripts

let states =
    def PARTIAL {
        def STATE s.LyingDown.FaceUp {
            category KnockDown
            stance Airborne
            keepFacingSide
            next s.WakeUp.FaceUp

            let isHardKnockDown = CalcOp.HasFlag(Arg1, HitFlags.HardKnockDown)

            let isSoftKnockDown =
                CalcOp.HasFlag(Arg1, HitFlags.SoftKnockDown) .& !isHardKnockDown

            let fromThrow = Var.LastReceived.attackMode === AttackMode.Throw

            action {
                If isSoftKnockDown { exit }

                Unless(isHardKnockDown .| fromThrow) {
                    If(
                        Query.Input {
                            command U
                            lenience 6<frames>
                            window 10<frames>
                            mash false
                            releaseCheck
                            strict
                        }
                    ) {
                        log "DEBUG: Quick wake-up!"
                        changeState s.WakeUp.FaceUpQuick
                    }
                }

                Repeat Var.Entity.downFrames { frame a.LyingDown.FaceUp 0 }
                frame a.LyingDown.FaceUp 0 15<times>
                Unless isHardKnockDown { call "CheckRoll" }
            }
        }

        def STATE s.LyingDown.FaceDown {
            category KnockDown
            stance Airborne
            keepFacingSide
            next s.WakeUp.FaceDown

            let isHardKnockDown = CalcOp.HasFlag(Arg1, HitFlags.HardKnockDown)

            let isSoftKnockDown =
                CalcOp.HasFlag(Arg1, HitFlags.SoftKnockDown) .& !isHardKnockDown

            action {
                If isSoftKnockDown { exit }
                frame a.LyingDown.FaceDown 0 15<times>
                Repeat Var.Entity.downFrames { frame }
                Unless isHardKnockDown { call "CheckRoll" }
            }
        }

        def STATE s.WakeUp.FaceUp {
            category WakeUp
            stance Stand
            keepFacingSide
            action { animate a.WakeUp.FaceUp 1 30 }
        }

        def STATE s.WakeUp.FaceDown {
            category WakeUp
            stance Stand
            keepFacingSide
            action { animate a.WakeUp.FaceDown 1 30 }
        }

        def STATE s.WakeUp.FaceUpQuick {
            category WakeUp
            stance Stand

            action {
                bringToFront
                animate a.WakeUp.FaceUpQuick 1 30
            }
        }

        def STATE s.WakeUp.RollForward {
            category Movement
            stance Stand
            allow Meta.Block.Grounded
            cost r.Stamina 100

            action {
                On ActionBegin { gainCooldown r.Stamina 60<frames> }

                bringToFront
                set Vel.X 10
                set Friction 0.2

                On ActionEnd {
                    clear Vel.X
                    clear Friction
                }

                effect Vfx.Blink Target.Self [ Translate(Axis.Y, 50) ]
                playSound Sfx.Blink
                animate a.WakeUp.RollForward 1 20
                throwReaction Punish
                animate a.WakeUp.RollForward 21 30
            }
        }

        def STATE s.WakeUp.RollBackward {
            category Movement
            stance Stand
            allow Meta.Block.Grounded
            cost r.Stamina 100

            action {
                On ActionBegin {
                    gainCooldown r.Stamina 60<frames>
                    flipFacingSide
                }

                bringToFront
                set Vel.X 10
                set Acc.X -0.4

                On ActionEnd {
                    clear Vel.X
                    clear Acc.X
                    applyFacingSide
                }

                effect Vfx.Blink Target.Self [ Translate(Axis.Y, 50) ]
                playSound Sfx.Blink
                animate a.WakeUp.RollForward 1 20
                throwReaction Punish
                animate a.WakeUp.RollForward 21 30
            }
        }

        def FUNC "CheckRoll" {
            If(
                Query.Input {
                    command F
                    hold 15<frames>
                }
            ) {
                requestState s.WakeUp.RollForward
                pass
            }

            If(
                Query.Input {
                    command B
                    hold 15<frames>
                }
            ) {
                requestState s.WakeUp.RollBackward
                pass
            }
        }
    }
