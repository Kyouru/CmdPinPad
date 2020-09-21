Imports System

Module Program

    Sub Main(ByVal Args() As String)
        Dim myPSerial As Object
        Dim WK As String = String.Empty

        myPSerial = CreateObject("HComPinpad.Pinpad")

        Dim puerto As String = String.Empty
        Dim velocidad As String = String.Empty
        Dim nroLlave As String = String.Empty
        Dim tipoEncriptacion As String = String.Empty

        Dim SCard As String
        Dim Resultado As String

        Dim iPin As Short
        Dim sPin As String

        Dim count As Integer = 0

        If Args.Count() = 11 Then
            For i As Integer = 1 To 9 Step 2
                If Args(i) = "-p" Then
                    puerto = Args(i + 1)
                ElseIf Args(i) = "-v" Then
                    velocidad = Args(i + 1)
                ElseIf Args(i) = "-n" Then
                    nroLlave = Args(i + 1)
                ElseIf Args(i) = "-e" Then
                    tipoEncriptacion = Args(i + 1)
                ElseIf Args(i) = "-wk" Then
                    WK = Args(i + 1)
                End If
            Next
        End If

        myPSerial.Connect(puerto, velocidad)

        myPSerial.TO_READ = CInt(600)
        myPSerial.TO_TIMEOUT = CInt(6000)

        SCard = myPSerial.ReadCard

        While SCard = "" 'And count < 10
            count += 1
            System.Threading.Thread.Sleep(1000)
            SCard = myPSerial.ReadCard
        End While

        myPSerial.ReadPinIni_D3D(nroLlave, WK, SCard, tipoEncriptacion)

        sPin = myPSerial.ReadPin(iPin)

        count = 0
        'iPin = 1: Lectura PINBLOCK OK!
        While sPin = "" And iPin <> 0 'And count < 10
            count += 1
            System.Threading.Thread.Sleep(1000)
            sPin = myPSerial.ReadPin(iPin)
        End While

        If sPin <> "" Then
            Resultado = UCase(sPin)
        ElseIf iPin = 0 Then 'iPin = 0: Se cancelo ingreso de clave
            Resultado = "CANCELADO"
        Else
            Resultado = "ERROR"
        End If

        Console.WriteLine(Resultado)
    End Sub

End Module
