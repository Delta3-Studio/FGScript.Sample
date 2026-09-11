namespace PrototypeFighter.Scripts.Bot

module p =
    let Gravity = 0.877
    let WalkSpeed = 3.525
    let BackWalkSpeed = -2.39
    let DashSpeed = 11.0
    let BackdashSpeed = -8.0

    let FallGravity = 0.67
    let KnockGravity = 0.65
    let SlowGravity = 0.6

    let Jump = {|
        VerticalSpeed = 18.0
        ForwardSpeed = 3.8
        BackwardSpeed = -3.0
    |}

    let StaminaRecovery = {|
        Normal = 0.8
        Slow = 0.4
        Fast = 1.2
    |}

    let JL = {|
        Normals = 4
        AirNormals = 6
        Specials = 10
        ExSpecials = 14
    |}

module r =
    let Energy = "Energy"
    let Stamina = "Stamina"

module bones =
    let Hips = "mixamorig_Hips"
    let Spine = "mixamorig_Spine"
    let Spine2 = "mixamorig_Spine2"

module v =
    let Attack = {|
        Light = [ "voice_atk_light_1"; "voice_atk_light_2" ]
        Medium = [ "voice_atk_medium_1"; "voice_atk_medium_2" ]
        Heavy = [ "voice_atk_heavy_1"; "voice_atk_heavy_2" ]
    |}

    let Jump = [ "voice_jump_1"; "voice_jump_2"; "voice_jump_3" ]

    let Hurt = {|
        Light = [ "voice_hurt_light_1"; "voice_hurt_light_2" ]
        Medium = [ "voice_hurt_medium_1"; "voice_hurt_medium_2" ]
        Heavy = [ "voice_hurt_heavy_1"; "voice_hurt_heavy_2" ]
        Counter = [ "voice_hurt_big_1"; "voice_hurt_big_2" ]
        Die = "voice_die_1"
    |}

    let Sp = {|
        Fireball = "voice_line_fireball"
        Tatsu = "voice_line_tatsu"
        Uppercut = "voice_line_uppercut"
    |}

    let Outro = {|
        Vitory = [ "voice_outro_victory_1" ]
        Annoyed = [ "annoyed_1"; "annoyed_2" ]
        Nope = "voice_line_nope"
        Taunt = "taunt_line_1"
    |}

[<System.Flags>]
type Tag =
    | None = 0
    | AutoThrowEscape = (1 <<< 0)
