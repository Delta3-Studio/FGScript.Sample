module PrototypeFighter.Scripts.Bot.Throws

open FGScript
open PrototypeFighter.Scripts
open Macros

let states =
    def PARTIAL {
        let techWindow = 7<frames>

        def STATE s.RegularThrow.EscapePushed {
            stance Stand
            category Movement

            action {
                stop
                fitAnimate (num 24) a.HitStun.Stand.Head 0 15
            }
        }

        def STATE s.RegularThrow.Escape {
            stance Stand
            category Movement

            action {
                On ActionBegin {
                    bringToFront
                    applyRepulsion Target.Opponent 24<frames> 120
                }

                frame a.RegularThrow.Escape 40
                effect Vfx.ThrowEscape At.Hand
                playSound Sfx.Throw.Escape
                fitAnimate (num 23) a.RegularThrow.Escape 41 70
            }
        }

        def STATE s.RegularThrow.Forward {
            stance Stand
            category Throw
            tags Tag.AutoThrowEscape

            trigger { command (LP + LK) }

            attack {
                throw
                skipNotifications
                set HitStop 6<frames>
            }

            action {
                On ActionBegin {
                    log Var.Entity.tags

                    Timer techWindow {
                        boundState
                        untag Tag.AutoThrowEscape
                    }
                }

                On DidThrow {
                    If(remote (Args.OnThrow.otherEntityId, hasTag Tag.AutoThrowEscape)) {
                        changeOtherState Args.OnThrow.otherEntityId s.RegularThrow.Escape
                        changeState s.RegularThrow.EscapePushed
                    }

                    capture At.Hint Args.OnThrow.otherEntityId
                    playSound Sfx.Throw.Connected
                    changeOtherState Args.OnThrow.otherEntityId s.RegularThrow.ForwardStun (args Args.OnThrow.reaction)
                    changeState s.RegularThrow.ForwardExec (args Args.OnThrow.reaction)
                }

                phase Startup
                animate a.RegularThrow.Forward 0 3
                phase Active
                animate a.RegularThrow.Forward 4 6
                phase Recovery
                playVoice v.Outro.Nope (Pitch 2<percent>)
                animate a.RegularThrow.Forward 7 29
            }
        }

        def STATE s.RegularThrow.ForwardExec {
            stance Stand
            category Throw
            tags Tag.AutoThrowEscape

            attack {
                damage 120
                gain r.Energy 40
                pushback OnHit 50
                pushbackTime 20<frames>
            }

            action {
                On ActionEnd {
                    release
                    clear Vel.X
                }

                disableLayerSorting
                set Vel.X 5.05
                animate a.RegularThrow.ForwardExec 0 1
                untag Tag.AutoThrowEscape
                If(Arg1 === HitReaction.Counter) { notify NotificationType.CounterHit }
                If(Arg1 === HitReaction.Punish) { notify NotificationType.PunishHit }
                animate a.RegularThrow.ForwardExec 2 15
                clear Vel.X
                animate a.RegularThrow.ForwardExec 16 50
                playVoice v.Attack.Medium
                animate a.RegularThrow.ForwardExec 51 53
                directDamage Target.Captured true
                animate a.RegularThrow.ForwardExec 54 90
            }
        }

        def STATE s.RegularThrow.ForwardStun {
            stance Stand
            category Hitstun
            skipMeta

            next
                s.HitStun.LandBack
                (args [
                    Args.HitStun.hitFlags
                    => Calc.Cond(Arg1 === HitReaction.Punish, HitFlags.HardKnockDown, HitFlags.None)
                ])

            action {
                On ActionBegin {
                    Timer techWindow {
                        boundState
                        disableLayerSorting
                        clearEvent EachFrame
                    }
                }

                Unless(Calc.AnyFlag(Args.OnThrow.reaction, HitReaction.Counter, HitReaction.Punish)) {
                    On EachFrame {
                        If(
                            Query.Input {
                                command (LP + LK)
                                lenience 3<frames>
                            }
                        ) {
                            changeOtherState Target.Captor s.RegularThrow.EscapePushed
                            releaseSelf
                            changeState s.RegularThrow.Escape
                        }
                    }
                }

                animate a.RegularThrow.ForwardStun 0 15
                On ActionEnd { flipFacingSide }
                animate a.RegularThrow.ForwardStun 16 54
            }
        }

        def STATE s.RegularThrow.Backward {
            stance Stand
            category Throw
            priority 1
            tags Tag.AutoThrowEscape

            trigger { command B (LP + LK) }

            attack {
                throw
                skipNotifications
                set HitStop 6<frames>
            }

            action {
                On ActionBegin {
                    Timer techWindow {
                        boundState
                        untag Tag.AutoThrowEscape
                    }
                }

                On DidThrow {
                    If(remote (Args.OnThrow.otherEntityId, hasTag Tag.AutoThrowEscape)) {
                        changeOtherState Args.OnThrow.otherEntityId s.RegularThrow.Escape
                        changeState s.RegularThrow.EscapePushed
                    }

                    capture At.Hint Args.OnThrow.otherEntityId
                    playSound Sfx.Throw.Connected
                    changeOtherState Args.OnThrow.otherEntityId s.RegularThrow.ForwardStun (args Args.OnThrow.reaction)
                    changeState s.RegularThrow.BackwardExec (args Args.OnThrow.reaction)
                }

                phase Startup
                animate a.RegularThrow.Forward 0 3
                phase Active
                animate a.RegularThrow.Forward 4 6
                phase Recovery
                playVoice v.Outro.Nope (Pitch 2<percent>)
                animate a.RegularThrow.Forward 7 29
            }
        }

        def STATE s.RegularThrow.BackwardExec {
            stance Stand
            category Throw
            tags Tag.AutoThrowEscape

            attack {
                damage 120
                gain r.Energy 40
                pushback OnHit 50
                pushbackTime 20<frames>
            }

            action {
                On ActionEnd {
                    release
                    clear Vel.X
                }

                disableLayerSorting
                set Vel.X 5.05
                animate a.RegularThrow.ForwardExec 0 1
                untag Tag.AutoThrowEscape
                If(Arg1 === HitReaction.Counter) { notify NotificationType.CounterHit }
                If(Arg1 === HitReaction.Punish) { notify NotificationType.PunishHit }
                animate a.RegularThrow.ForwardExec 2 15
                clear Vel.X
                flipFacingSide
                animate a.RegularThrow.ForwardExec 16 50
                playVoice v.Attack.Medium
                animate a.RegularThrow.ForwardExec 51 53
                directDamage Target.Captured true
                animate a.RegularThrow.ForwardExec 54 90
            }
        }
    }
