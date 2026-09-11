module PrototypeFighter.Scripts.Bot.Events

open FGScript


let states =
    def PARTIAL {
        On INIT { log "INIT: Fight Bot" }

        On NEUTRAL {
            clear Acc
            send Signal.Children (%"ChildMessageValue" + 10)
        }

        On ROUND { log "ROUND: BEGIN " }

        //On REVERSAL { log "REVERSAL" Arg1 }
        //On RECOVERY { log "RECOVERY" }
        //On EachFrame { If(Var.comboStunTime .> 0) { log "COMBO STUN TIME" Var.comboStunTime Var.comboReceivedHits } }

        On Died { playVoice v.Hurt.Die }
        On RoundEnd { log "ROUND: END" Arg1 }
        On TouchWall { log "TOUCHED: Wall" }
        On TouchCorner { log "TOUCHED: Corner" }
        On KnockedDown { log "KNOCKED DOWN: Happened" }

        On(Children, LeaveScreen) { log "CHILD: Leave Screen, Id =" Args.Last }

        On Signal.Opponent {
            If(Arg1 == 1) {
                log "OPPONENT: Ping Received" Arg2 Args.Last
                Timer 30<frames> { send Signal.Opponent (num 2) Arg2 }
                pass
            }

            If(Arg1 == 2) {
                log "OPPONENT: Pong Received, latency =" (Var.currentRoundFrame - Arg2) Args.Last
                pass
            }
        }

        On Signal.Children {
            log "MSG: Child" Arg1 Args.Last
            set "ChildMessageValue" Arg1
        }
    }
