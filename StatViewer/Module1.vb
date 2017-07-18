Module Module1
    Enum RBTeamStatIndex As Integer
        Kickouts = 1
        OwnKickoutsWon = 2
        KickoutsWonClean = 3
        KickoutsWonBroken = 4
        HandPasses = 5
        FootPasses = 6
        FreesConcededOwnHalf = 7
        FreesConcededOppHalf = 8
        ScoringChanceFromPlay = 9
        ScoreFromPlay = 10
        Wides = 11
        FreeTaken = 12
        FreeScored = 13
        TurnoversWon = 14
        FortyFive = 15
        BlackCards = 16
        YellowCards = 17
        RedCards = 18
        Hooks = 5
        Blocks = 6
        SubsUsed = 35
        Marks = 36

        'FreeTakenNotUsed = 0
        'Kickouts = 1
        'OwnKickoutsWon = 2
        'KickoutsWonClean = 3
        'KickoutsWonBroken = 4
        'Wides = 5
        'FreesConcededOwnHalf = 6
        'FreesConcededOppHalf = 7
        'HandPasses = 8
        'FootPasses = 9
        'ScoringChanceFromPlay = 10
        'ScoreFromPlay = 11
        'FreeScored = 12
        'SubsUsed = 13
        'FortyFive = 14
        'TurnoversWon = 15
        'BlackCards = 16
        'YellowCards = 17
        'RedCards = 18

        'Kickouts = 1
        'OwnKickoutsWon = 2
        'KickoutsWonClean = 3
        'KickoutsWonBroken = 4
        'Wides = 5
        'FreesConcededOwnHalf = 6
        'FreesConcededOppHalf = 7
        'YellowCards = 8
        'RedCards = 9
        'BlackCards = 10
        'HandPasses = 11
        'FootPasses = 12
        'ScoringChanceFromPlay = 13
        'ScoreFromPlay = 14
        'FreeTaken = 15
        'FreeScored = 16
        'SubsUsed = 17
        'FortyFives = 18
        'LostPossession = 19
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
        TurnoversWon = 14
        HandPasses = 15
        FootPasses = 16
        FortyFives = 17
        Marks = 18
    End Enum
    Public RBTeamStats(50) As clsRBTeamStat 'spare
End Module
