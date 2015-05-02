﻿Module Module1
    Enum RBTeamStatIndex As Integer
        Kickouts = 1
        OwnKickoutsWon = 2
        KickoutsWonClean = 3
        KickoutsWonBroken = 4
        Wides = 5
        FreesConcededOwnHalf = 6
        FreesConcededOppHalf = 7
        YellowCards = 8
        RedCards = 9
        BlackCards = 10
        HandPasses = 11
        FootPasses = 12
        ScoringChanceFromPlay = 13
        ScoreFromPlay = 14
        FreeTaken = 15
        FreeScored = 16
        SubsUsed = 17
        FortyFives = 18
        LostPossession = 19
    End Enum
    'Enum RTEGAAHTeamStatIndex As Integer
    '    Puckouts = 1
    '    OwnPuckoutsWon = 2
    '    PuckoutsWonClean = 3
    '    PuckoutsWonBroken = 4
    '    Wides = 5
    '    FreesConcededOwnHalf = 6
    '    FreesConcededOppHalf = 7
    '    YellowCards = 8
    '    RedCards = 9
    '    BlackCards = 10
    '    HandPasses = 11
    '    StickPasses = 12
    '    ScoringChanceFromPlay = 13
    '    ScoreFromPlay = 14
    '    FreeTaken = 15
    '    FreeScored = 16
    '    SubsUsed = 17
    '    SixtyFives = 18
    '    LostPossession = 19
    'End Enum

    Enum SVFStatIndex As Integer
        Wides = 1
        ScoringChanges = 2
        FreesConverted = 3
        ScoresFromPlay = 4
        OwnKickoutsWon = 5
        KickoutsWonClean = 6
        KickoutsWonBroken = 7
        FreesConcededOwnHalf = 8
        FreesConcededOppHalf = 9
        Blacks = 10
        Yellows = 11
        Reds = 12
        SubsMade = 13
        LostPossession = 14
        HandPasses = 15
        FootPasses = 16
        FortyFives = 17
    End Enum
    Public RBTeamStats(50) As clsRBTeamStat 'spare
End Module
