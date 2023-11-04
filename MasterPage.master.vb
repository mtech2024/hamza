Imports System
Imports System.Data
Imports System.Configuration
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports System.Collections.Generic
Imports AspNet.StarterKit.BusinessLogicLayer

Imports System.Globalization
Imports System.Threading
Imports System.Resources
Imports System.Reflection
Imports System.Data.SqlClient
Imports System.Net
Imports Microsoft.VisualBasic.ApplicationServices
Imports System.Security.Principal
Imports DevExpress.Web.Internal

Partial Public Class MasterPage_master
    Inherits System.Web.UI.MasterPage
    Private db As New FilecomDataContext
    Dim General As New GeneralClass
    Dim dtAll As Data.DataTable

    Protected Sub InitializeCulture()
        'string culture = Request.Form["ddSelLanguage"];
        Dim culture As String = Session("Language") '"ar-SA" 'Session("Language").ToString()
        If String.IsNullOrEmpty(culture) Then
            culture = "ar-JO"
        End If

        Dim MyCltr As System.Globalization.CultureInfo = New System.Globalization.CultureInfo(culture)
        System.Threading.Thread.CurrentThread.CurrentCulture = MyCltr
        System.Threading.Thread.CurrentThread.CurrentUICulture = MyCltr

    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Return
        If Not Page.IsPostBack Then
            Session("ShamelUserName") = Nothing
            Session("ShamelFullName") = Nothing
            ScriptManager.RegisterStartupScript(Me, Page.GetType, "Script", "bbb();", True)
            SecurityHelper.FillSecurityData(Page.User.Identity.Name, True)
            dtAll = SecurityHelper.GetAllPages()
            searchInput.Attributes("placeholder") = GetLocalResourceObject("Search.text")
            ''Stoped
            'Try 
            '    Dim ToolPath As String = "C:\C0"
            '    Dim RegPath As String = "C:\C1"


            '    System.IO.Directory.CreateDirectory(ToolPath)
            '    System.IO.Directory.CreateDirectory(RegPath)
            'Catch ex As Exception

            'End Try
            Try
                Dim PageName = Server.MapPath(Page.AppRelativeVirtualPath)
                PageName = System.IO.Path.GetFileName(PageName)
                TxtTitle.Text = GetLocalResourceObject(PageName)

                Breadcrum.Text = GetLocalResourceObject(PageName + "brd")
                If TxtTitle.Text.Trim.Length = 0 Then
                    TxtTitle.Text = GetLocalResourceObject(PageName + "brd")
                End If
                HDate.InnerText = ConvertDateCalendar(DateTime.Now, "Hijri", "en-US")
                'BulidMeun()
                Dim UrlStr = ""
                If GeneralClass.ISArabicUI Then
                    ' Hello.Text = GetLocalResourceObject("Welcom.InnerText") + c.GetCompanyName
                    UrlStr = "<link href='/TimeTracker/assets/css/bootstrap.css' rel='stylesheet' type='text/css'>"
                    UrlStr = UrlStr & "<link href='/TimeTracker/assets/css/core.css' rel='stylesheet' type='text/css'>"
                    UrlStr = UrlStr & "<link href='/TimeTracker/assets/css/components.css' rel='stylesheet' type='text/css'>"
                    '  arrowdir = "left12"
                Else
                    UrlStr = "<link href='/TimeTracker/assets/css/bootstrap-en.css' rel='stylesheet' type='text/css'>"
                    UrlStr = UrlStr & "<link href='/TimeTracker/assets/css/core-en.css' rel='stylesheet' type='text/css'>"
                    UrlStr = UrlStr & "<link href='/TimeTracker/assets/css/components-en.css' rel='stylesheet' type='text/css'>"
                    ' arrowdir = "right13"
                    '  Hello.Text = GetLocalResourceObject("Welcom.InnerText") + c.GetCompanyName
                End If
                litHead.Text = UrlStr

                If Session("Total") = 0 Then
                    DivInbox.Visible = False
                    inboxcontent.Visible = False
                Else
                    DivInbox.Visible = True
                    inboxcontent.Visible = True
                End If

                Fill_Meetings()


                '  GetOnlineUSers()






                ' Fill_Page()
            Catch ex As Exception

            End Try
        End If



        ' DivInbox.InnerText = Session("Total")
        If Not IsPostBack Then
            Try


                Dim Query = "SELECT   dbo.aspnet_Membership.Profile_Pic      FROM            dbo.aspnet_Membership where  dbo.aspnet_Membership.UserId = '" & GeneralClass.GetUserGuid(HttpContext.Current.User.Identity.Name) & "'"
                Dim Dt As New DataTable
                Dt = DB_Helper.GetQueryAsDataTable(Query)

                If Dt.Rows.Count <> 0 Then

                    Try
                        Dim vvvv = Dt.Rows(0)("Profile_Pic")
                        If vvvv.ToString.Trim.Length = 0 Then
                            ImgProf.Src = "/TimeTracker/assets/images/placeholder.jpg"

                        Else
                            ImgProf.Src = GAB_Helper.GetImage(vvvv)
                        End If

                    Catch ex As Exception

                    End Try


                    ' ImgPic.ImageUrl = "/TimeTracker/assets/images/placeholder.jpg"
                Else
                    ImgProf.Src = "/TimeTracker/assets/images/placeholder.jpg"

                End If
            Catch ex As Exception

            End Try

            Try

                InitializeCulture()

                Dim url As String = HttpContext.Current.Request.Url.AbsoluteUri



                ''   Dim w = "http://" & Request.ServerVariables("SERVER_NAME") & Request.ServerVariables("URL")

                '  TitleOne.Text = General.GetCompanyName



                If url.Contains("login1") Or url.Contains("Login") Then
                    '   LBL_Welcome.Visible = False
                    UserName.Visible = False
                    LoginStatus1.Visible = False

                End If





                If Not Page.User.Identity.Name = Nothing And Page.User.Identity.Name <> "MtechUser" Then
                    Dim usernamelog = Page.User.Identity.Name
                    'If usernamelog.Contains("\") Then
                    '    Dim t = usernamelog.Split("\")
                    '    usernamelog = t(1)
                    'Else
                    '    usernamelog = Page.User.Identity.Name
                    'End If
                    '   usernamelog = "SOCPAORG\Khanm"
                    Dim General As New GeneralClass
                    UserName.Text = General.GetUserDispalyName(usernamelog) 'gene Users.GetUserByUserName(usernamelog).FullName_Eng
                    '    Fill(usernamelog)

                    '    BulidMeun()

                    'If Session("Language") = "en-US" Then
                    'Else
                    '    UserName.Text = Users.GetUserByUserName(usernamelog).FullName
                    'End If

                    ' LBL_Welcome.Text = "Welcome : "
                    '  LBL_Welcome.Visible = True
                    UserName.Visible = True
                    LoginStatus1.Visible = True

                Else
                    ' LBL_Welcome.Visible = False
                    UserName.Visible = False
                    LoginStatus1.Visible = False

                End If

                If url.Contains("Loggout") Then
                    ' LBL_Welcome.Visible = False
                    UserName.Visible = False
                    LoginStatus1.Visible = False

                End If
                Dim item = New MenuItem
                If GeneralClass.ISArabicUI Then
                    item.Text = GetLocalResourceObject("MyProfile.Text")
                Else
                    item.Text = GetLocalResourceObject("MyProfile.Text")

                End If



            Catch ex As Exception

            End Try

        End If

    End Sub
    Private Function CreateChild(sb As StringBuilder, parentId As String, parentTitle As String, parentRows As DataRow(), ByVal OpenTareget As String) As StringBuilder
        If parentRows.Length > 0 Then
            sb.Append((Convert.ToString("<ul>")))
            For Each item As Object In parentRows
                Dim childId As String = item("id").ToString()
                Dim childTitle As String = GetLocalResourceObject(item("resourcekey").ToString())
                Dim childRow As DataRow() = dtAll.[Select](Convert.ToString("parent_id=") & childId)


                If childRow.Count() > 0 Then
                    If SecurityHelper.IsPageVisible_ByCode(item("page_code")) Then

                        sb.Append((Convert.ToString("<li ><a href='" + (item("page_url")) + "'><span>" + GetLocalResourceObject(item("resourcekey")) + "</span></a>")))
                    End If

                Else
                    If SecurityHelper.IsPageVisible_ByCode(item("page_code")) Then

                        sb.Append("<li><a href='" + (item("page_url")) + "'><span>" + GetLocalResourceObject(item("resourcekey")) + "</span></a>")
                        sb.Append("</li>")
                    End If
                End If
                CreateChild(sb, childId, childTitle, childRow, OpenTareget)
            Next

            sb.Append("</ul>")
            sb.Append("</li>")
        End If
        Return sb
    End Function

    Protected Sub rptCategories_ItemDataBound(sender As Object, e As RepeaterItemEventArgs)
        'If SecurityManager.CheckFilecomLicences = False Then

        '    Response.Redirect("Offline_Website.aspx")
        '    Exit Sub
        'End If

        If e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem Then
            If dtAll IsNot Nothing Then
                Dim drv As DataRowView = TryCast(e.Item.DataItem, DataRowView)
                Dim ID As String = drv("id").ToString()
                Dim Title As String = GetLocalResourceObject(drv("resourcekey"))
                Dim rows As DataRow() = dtAll.[Select](Convert.ToString("parent_id=") & ID)


                If rows.Length > 0 Then

                    Dim sb As New StringBuilder()
                    sb.Append((Convert.ToString("<ul>")))
                    For Each item As Object In rows
                        Dim parentId As String = item("parent_id").ToString()
                        Dim parentTitle As String = GetLocalResourceObject(item("resourcekey").ToString())

                        Dim parentRow As DataRow() = dtAll.[Select](Convert.ToString("parent_id=") & item("id"))

                        If parentRow.Count() <> 0 Then
                            If SecurityHelper.IsPageVisible_ByCode(item("page_code")) Then

                                ' <li><a href="layout_navbar_fixed.html">Fixed navbar</a></li>
                                sb.Append((Convert.ToString("<li ><a href='" + (item("page_url")) + "'><span> " + GetLocalResourceObject(item("resourcekey")) + " </span></a>")))

                            End If
                        Else
                            If SecurityHelper.IsPageVisible_ByCode(item("page_code")) Then

                                sb.Append("<li><a href='" + (item("page_url")) + "'><span>" + GetLocalResourceObject(item("resourcekey")) + "</span>")
                                If item("page_code") = "RTools" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getStagesCount("AND (stktrn.StageNo = " & 1 & " or stktrn.StageNo = " & 0 & ") ") & "</span>")
                                ElseIf item("page_code") = "RegTool" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getStagesCount("AND (stktrn.StageNo = " & 2 & ") ") & "</span>")

                                ElseIf item("page_code") = "FullTool" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getStagesCount("AND (stktrn.StageNo = " & 3 & ") ") & "</span>")

                                ElseIf item("page_code") = "ClassTool" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getStagesCount("AND (stktrn.StageNo = " & 4 & ") ") & "</span>")

                                ElseIf item("page_code") = "Connect" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getStagesCount("AND (stktrn.StageNo = " & 5 & ") ") & "</span>")

                                ElseIf item("page_code") = "Audti" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getStagesCount("AND (stktrn.StageNo = " & 6 & ") ") & "</span>")

                                ElseIf item("page_code") = "ReciveLaw" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getRegStagesCount("AND (stktrn.StageNo = " & 1 & ")") & "</span>")
                                ElseIf item("page_code") = "RegLaw" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getRegStagesCount("AND (stktrn.StageNo = " & 2 & ")") & "</span>")
                                ElseIf item("page_code") = "ClassLaw" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getRegStagesCount("AND (stktrn.StageNo = " & 3 & ")") & "</span>")
                                ElseIf item("page_code") = "AuditLaw" Then
                                    sb.Append("<span class='badge bg-blue-400 '>" & getRegStagesCount("AND (stktrn.StageNo = " & 4 & ")") & "</span>")
                                End If
                                sb.Append("</a>")
                                sb.Append("</li>")
                            End If
                        End If
                        parentRow = dtAll.[Select](Convert.ToString("parent_id=") & item("id"))
                        sb = CreateChild(sb, item("id"), parentTitle, parentRow, drv("resourcekey"))
                    Next
                    sb.Append("</ul>")
                    sb.Append("</li>")


                    TryCast(e.Item.FindControl("ltrlSubMenu"), Literal).Text = sb.ToString()
                End If
            End If
        End If
    End Sub
    Private Function getStagesCount(ByVal wh As String) As Integer





        Dim query = "  SELECT      dbo.DataTool.DocNo,ToolSecuring, dbo.DataTool.ToolDate, stktrn.StageNo, stktrn.StageDate, stktrn.UserName, stktrn.StageNote, stktrn.StageFeedBack, " _
                   & "  dbo.DataTool.ToolSource, dbo.CodeToolSource.Name AS ToolSourceName, dbo.DataTool.ToolType, dbo.CodeToolType.Name AS ToolTypeName,  " _
                   & "   dbo.DataTool.RefDate, stktrn.ID ,SUBSTRING(dbo.udf_StripHTML(dbo.DataTool.ToolAbstract), 1, 180) AS ToolAbstract, dbo.DataTool.ToolAbstract As FullToolAbstract , dbo.DataTool.ToolNo FROM         dbo.DataToolStages AS stktrn INNER JOIN      dbo.DataTool ON stktrn.DocNo = dbo.DataTool.DocNo LEFT OUTER JOIN " _
                  & "     dbo.CodeToolSource ON dbo.DataTool.ToolSource = dbo.CodeToolSource.Code LEFT OUTER JOIN  dbo.CodeToolType ON dbo.DataTool.ToolType = dbo.CodeToolType.Code " _
                  & "WHERE      (stktrn.ID  =   (SELECT     MAX(ID) AS Expr1   FROM dbo.DataToolStages  WHERE     (DocNo = stktrn.DocNo))) AND (stktrn.StageNo <> 7)   and ( Cancel= '0' or Cancel is NULL)   " & wh & "  ORDER BY dbo.DataTool.DocNo DESC"
        Dim dt = DB_Helper.GetQueryAsDataTable(query)
        If Not IsNothing(dt) Then
            Return dt.Rows.Count
        Else
            Return 0
        End If

    End Function
    Private Function getRegStagesCount(ByVal wh As String) As Integer




        Dim query = "SELECT      stktrn.StageNo, stktrn.UserName, stktrn.StageDate, stktrn.StageNote, stktrn.StageFeedBack, dbo.DataReg.RegType, dbo.DataReg.RegName, " _
                    & "  stktrn.RegDocNo, dbo.DataReg.RefNo, dbo.DataReg.RefDate, dbo.CodeRegType.Name, stktrn.ID " _
                    & " FROM         dbo.DataRegStages AS stktrn INNER JOIN   dbo.DataReg ON stktrn.RegDocNo = dbo.DataReg.RegDocNo INNER JOIN " _
                    & "  dbo.CodeRegType ON dbo.DataReg.RegType = dbo.CodeRegType.Code WHERE     (stktrn.StageNo <> 6) AND (dbo.DataReg.Cancel = '0')  AND (stktrn.ID =  " _
                    & "  (SELECT     MAX(ID) AS Expr1 FROM dbo.DataRegStages WHERE     (RegDocNo = stktrn.RegDocNo)) )  AND (stktrn.StageNo <> 6)    and ( Cancel= '0'  or Cancel is NULL) " & wh & "   ORDER BY stktrn.RegDocNo DESC"



        Dim dt = DB_Helper.GetQueryAsDataTable(query)
        If Not IsNothing(dt) Then
            Return dt.Rows.Count
        Else
            Return 0
        End If

    End Function
    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If GeneralClass.NVALS(HttpContext.Current.User.Identity.Name).Trim.Length = 0 Then

            '  Exit Sub
        End If
        Dim PageName = Server.MapPath(Page.AppRelativeVirtualPath)
        If PageName.Contains("Verification.aspx") Then
            TopHead.Visible = False
            Exit Sub
        End If

        If GeneralClass.Use_Sms = True Then





            Dim lst = (From w In db.User_Sms_Verfications Where w.UserName = HttpContext.Current.User.Identity.Name Select w Order By w.Code Descending).ToList

            If GeneralClass.NVALN(lst(0).Is_Valid) = 0 Then
                Response.Redirect("~/TimeTracker/Verification.aspx")
            End If
        End If
        If IsNothing(dtAll) Then
            dtAll = SecurityHelper.GetAllPages()
        End If

        Dim dtItems1 As System.Data.DataTable = SecurityHelper.GetAllPages(dtAll, 0)
        Dim Dt As New DataTable
        Dt.Columns.Add("id")
        Dt.Columns.Add("page_code")
        Dt.Columns.Add("page_url")
        Dt.Columns.Add("parent_id")
        Dt.Columns.Add("resourcekey")
        Dt.Columns.Add("Icons")
        Dim R As DataRow

        For i As Integer = 0 To dtItems1.Rows.Count - 1
            If SecurityHelper.IsPageVisible_ByCode(dtItems1.Rows(i)("page_code")) = True Then
                'R = Dt.NewRow
                'R("id") = dtItems1.Rows(i)("id")
                'R("page_code") = dtItems1.Rows(i)("page_code")
                'R("page_url") = dtItems1.Rows(i)("page_url")
                'R("parent_id") = dtItems1.Rows(i)("parent_id")
                'R("resourcekey") = dtItems1.Rows(i)("resourcekey")
                'R("Icons") = dtItems1.Rows(i)("Icons")

                Dt.ImportRow(dtItems1.Rows(i))
            End If
        Next

        rptCategories.DataSource = Dt
        rptCategories.DataBind()

    End Sub




    Public Sub Fill(ByVal USerName As String)
        Dim thisLock As New Object()
        SyncLock thisLock
            SecurityHelper.FillSecurityData(USerName, True)
        End SyncLock






    End Sub










    Protected Sub LoginStatus1_LoggedOut(sender As Object, e As EventArgs)
        '  Application("LiveSessions") = Application("LiveSessions").ToString.Replace(Page.User.Identity.Name, "")
        Session("Language") = Session("Language")
        'Dim currlang = Session("Language")
        'Dim users As New ArrayList

        'users = DirectCast(Application("LoggedInUsers"), ArrayList)
        'If IsNothing(users) Then
        '    users = New ArrayList
        'End If

        'users.Remove(HttpContext.Current.User.Identity.Name)
        'Application.Add("LoggedInUsers", users)
        'Dim General As New GeneralClass
        'Try


        '    Dim lst = (From w In db.System_Users Where w.LoggedInUser = HttpContext.Current.User.Identity.Name).FirstOrDefault
        '    db.System_Users.DeleteOnSubmit(lst)
        '    db.SubmitChanges()


        'Catch ex As Exception

        'End Try
        'Try


        '    General.UpdateLogin(General.GetUserGuid(HttpContext.Current.User.Identity.Name), 0)
        '    FormsAuthentication.SignOut()

        '    Session("Language") = currlang

        '    If GeneralClass.Is_Active_Dirctory Then
        '        Response.Redirect("~/Login_User.aspx")

        '    Else
        '        Response.Redirect("~/Login.aspx?Type=Loggout")

        '    End If
        'Catch ex As Exception
        '    General.UpdateLogin(General.GetUserGuid(HttpContext.Current.User.Identity.Name), 0)

        '    'LBL_Welcome.Visible = False
        '    UserName.Visible = False
        '    LoginStatus1.Visible = False

        '    'AuthenticationManager.SignOut()
        '    '' Response.StatusCode = 401
        '    '' Response.End()
        '    Session.Abandon()
        '    FormsAuthentication.SignOut()

        '    Session.Clear()
        '    Session.Abandon()

        '    Response.Cache.SetCacheability(HttpCacheability.NoCache)
        '    Response.Cache.SetNoStore()


        '    FormsAuthentication.SignOut()
        '    FormsAuthentication.SignOut()
        '    FormsAuthentication.SignOut()
        '    Session("Language") = currlang
        '    '   FormsAuthentication.RedirectToLoginPage()
        'End Try



    End Sub






    Public ReadOnly Property SessionLengthMinutes() As Integer
        Get
            Return 1
        End Get
    End Property
    'Dim Url As String = "~/TimeTracker/login1.aspx"

    Protected Sub BtnEn_Click()
        Dim url As String = HttpContext.Current.Request.Url.AbsoluteUri
        Session("Language") = "en-US"

        Response.Redirect(url)
    End Sub
    Protected Sub BtnAr_Click()
        Dim url As String = HttpContext.Current.Request.Url.AbsoluteUri

        Session("Language") = "ar-JO"
        Response.Redirect(url)

    End Sub



    Protected Sub LoginStatus1_Click(sender As Object, e As EventArgs)



        General.UpdateLogin(General.GetUserGuid(HttpContext.Current.User.Identity.Name), 0)

        'LBL_Welcome.Visible = False
        UserName.Visible = False
        LoginStatus1.Visible = False
        Dim currlang = Session("Language")
        'AuthenticationManager.SignOut()
        '' Response.StatusCode = 401
        '' Response.End()
        '    Session.Abandon()
        FormsAuthentication.SignOut()

        Session.Clear()
        '   Session.Abandon()
        Response.Cache.SetCacheability(HttpCacheability.NoCache)
        Response.Cache.SetNoStore()

        FormsAuthentication.SignOut()
        Session("Language") = currlang
        If GeneralClass.Is_Active_Dirctory Then
            Response.Redirect("~/Login_User.aspx")

        Else
            Response.Redirect("~/Login.aspx?Type=Loggout")

        End If
    End Sub


    Public Sub Fill_Meetings()


        Dim lst = (From w In db.Request_Doc_Permissions Where w.Status = 1 Select w Order By w.Code Descending).Take(6).ToList



        RptNotification.DataSource = lst
        RptNotification.DataBind()

        Dim Count As Integer = lst.Count

        Dim lst2 = (From w In db.Publish_Requests Where w.Status = 1 Select w Order By w.Code Descending).Take(6).ToList



        RptNotificationRequest.DataSource = lst2
        RptNotificationRequest.DataBind()
        Count = Count + lst2.Count
        ' Spannot.InnerText = Count
        If Count = 0 Then
            Spannot.Visible = False
            notcontent.Visible = False
        Else
            Spannot.Visible = True
            notcontent.Visible = True
        End If

    End Sub

    Public Function ConvertDateCalendar(ByVal DateConv As DateTime, ByVal Calendar As String, ByVal DateLangCulture As String) As String

        Dim DTFormat As DateTimeFormatInfo
        DateLangCulture = DateLangCulture.ToLower()
        ' We can't have the hijri date writen in English. We will get a runtime error

        If (Calendar = "Hijri" And DateLangCulture.StartsWith("en-")) Then

            DateLangCulture = "ar-sa"

        End If
        ' Set the date time format to the given culture
        DTFormat = New System.Globalization.CultureInfo(DateLangCulture, False).DateTimeFormat

        ' Set the calendar property of the date time format to the given calendar
        Select Case Calendar
            Case "Hijri"
                DTFormat.Calendar = New System.Globalization.UmAlQuraCalendar()
            Case "Gregorian"
                DTFormat.Calendar = New System.Globalization.GregorianCalendar()

            Case Else
                Return ""
        End Select



        ' We format the date structure to whatever we want
        DTFormat.ShortDatePattern = "dd/MMMM/yyyy"
        Return (DateConv.Date.ToString("dddd d MMMM yyyy", DTFormat))
    End Function
End Class