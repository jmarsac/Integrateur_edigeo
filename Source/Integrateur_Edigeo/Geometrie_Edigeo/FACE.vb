﻿Public Class FACE
    Inherits Geometrie

    Private mListeArc As System.Collections.Generic.List(Of ARC)
    Public Property ListeARC() As System.Collections.Generic.List(Of ARC)
        Get
            Return mListeArc
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of ARC))
            mListeArc = value
        End Set
    End Property
    Private mNbAttribut As Integer
    Public Property NbAttribut() As Integer
        Get
            Return mNbAttribut
        End Get
        Set(ByVal value As Integer)
            mNbAttribut = value
            If value > 0 Then
                mSCDAttribut = New ListeDescripteurReference

                mValeurAtt = New System.Collections.Generic.List(Of String)
            End If
        End Set
    End Property


    Private mSCDAttribut As ListeDescripteurReference
    Public Property SCDAttribut() As ListeDescripteurReference
        Get
            Return mSCDAttribut
        End Get
        Set(ByVal value As ListeDescripteurReference)
            mSCDAttribut = value
        End Set
    End Property

    Private mValeurAtt As System.Collections.Generic.List(Of String)
    Public Property ValeurAtt() As System.Collections.Generic.List(Of String)
        Get
            Return mValeurAtt
        End Get
        Set(ByVal value As System.Collections.Generic.List(Of String))
            mValeurAtt = value
        End Set
    End Property
    Public Sub New()
        mListeArc = New System.Collections.Generic.List(Of ARC)
    End Sub
    Private mPolygone As POLYGON
    Public ReadOnly Property GetPolygone() As POLYGON
        Get
            mPolygone = New POLYGON


            If ListeARC.Count = 0 Then
                Return Nothing
            End If

            If ListeARC(0).NoeudInitial Is Nothing Then
                'If ListeARC.Count > 1 Then
                '*****************************************************

                'ListeARC.Sort(AddressOf CompareSurface)




                For Each ar In ListeARC
                    'mPolygone.ListeRing.Add(ar.Points)
                    '*************************************************************

                    ar.NoeudInitial = New NOEUD
                    ar.NoeudInitial.X = ar.Points(0).X
                    ar.NoeudInitial.Y = ar.Points(0).Y
                    ar.NoeudFinal = New NOEUD
                    ar.NoeudFinal.X = ar.Points(ar.Points.Count - 1).X
                    ar.NoeudFinal.Y = ar.Points(ar.Points.Count - 1).Y

                Next
            End If


            Dim tl As New System.Collections.Generic.List(Of Coordonee)

            Dim tempArc As New System.Collections.Generic.List(Of ARC)

            For Each a In ListeARC
                tempArc.Add(a)
            Next

            Dim count As Integer = ListeARC.Count
            Dim arret As Boolean = False


            Dim NF As New NOEUD
            Dim NI As New NOEUD

            Do While count > 0
                arret = False
                tl = New System.Collections.Generic.List(Of Coordonee)
                'NF = New NOEUD
                'NI = New NOEUD                    
                NI = tempArc(0).NoeudInitial
                NF = tempArc(0).NoeudFinal
                tl.AddRange(tempArc(0).Points)
                count = count - 1
                tempArc.Remove(tempArc(0))
                If tempArc.Count = 0 Then
                    mPolygone.ListeRing.Add(tl)
                    Exit Do
                End If
                Do While Not arret
                    Dim AR As ARC = FindArcNoeud(tempArc, NF)

                    If AR Is Nothing Then
                        'MsgBox("on a un souci")
                        arret = True
                    Else
                        tl.AddRange(AR.Points)
                        count = count - 1
                        'NF = New NOEUD
                        NF = AR.NoeudFinal
                        arret = NF.Point.Egale(NI.Point)
                        'If NF.Point.Egale(NI.Point) Then
                        '    arret = True
                        'End If
                    End If

                Loop

                mPolygone.ListeRing.Add(tl)

            Loop


            'End If

            mPolygone.ListeRing.Sort(AddressOf CompareSurfaceRing)

            mPolygone.CheckAndRepareGeometry()

            Return mPolygone
        End Get
    End Property

    Private Function FindArcNoeud(ByVal Mliste As System.Collections.Generic.List(Of ARC), ByVal N As NOEUD) As ARC
        For Each ar In Mliste
            Dim resar As New ARC
            If ar.NoeudInitial.Point.Egale(N.Point) Then
                resar.Points = ar.Points
                resar.NoeudInitial = ar.NoeudInitial
                resar.NoeudFinal = ar.NoeudFinal
                Mliste.Remove(ar)
                Return resar
            End If

            If ar.NoeudFinal.Point.Egale(N.Point) Then

                For i = ar.Points.Count - 1 To 0 Step -1
                    resar.Points.Add(ar.Points(i))
                Next

                resar.NoeudFinal = ar.NoeudInitial
                resar.NoeudInitial = ar.NoeudFinal
                Mliste.Remove(ar)
                Return resar
            End If
        Next

        Return Nothing
    End Function

    Private Function CompareSurfaceRing(ByVal P1 As System.Collections.Generic.List(Of Coordonee), ByVal P2 As System.Collections.Generic.List(Of Coordonee)) As Integer
        Dim S1 As Double = Surface(P1)
        Dim s2 As Double = Surface(P2)
        If S1 > s2 Then
            Return -1

        Else
            If S1 = s2 Then
                Return 0
            Else
                Return 1
            End If

        End If
    End Function

    Public Function Surface(ByVal ListePoint As System.Collections.Generic.List(Of Coordonee)) As Double
        Dim SA As Double = 0

        For i = 1 To ListePoint.Count - 1
            SA = SA + (ListePoint(i).X - ListePoint(i - 1).X) * (ListePoint(i).Y + ListePoint(i - 1).Y)
        Next

        SA = SA + (ListePoint(0).X - ListePoint(ListePoint.Count - 1).X) * (ListePoint(0).Y + ListePoint(ListePoint.Count - 1).Y)

        SA = Math.Abs(SA / 2)

        Return SA

    End Function

    Private Function CompareSurface(ByVal P1 As ARC, ByVal P2 As ARC) As Integer
        Dim S1 As Double = Surface(P1.Points)
        Dim s2 As Double = Surface(P2.Points)
        If S1 > s2 Then
            Return -1

        Else
            If S1 = s2 Then
                Return 0
            Else
                Return 1
            End If

        End If
    End Function
End Class
