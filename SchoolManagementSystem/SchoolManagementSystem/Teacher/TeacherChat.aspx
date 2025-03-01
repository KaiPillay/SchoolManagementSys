<%@ Page Title="Teacher Chat" Language="C#" MasterPageFile="~/Teacher/TeacherMst.Master" AutoEventWireup="true" CodeBehind="TeacherChat.aspx.cs" Inherits="SchoolManagementSystem.Teacher.TeacherChat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="chat-container">
        <h3>Teacher Chat</h3>

        <!-- User selection dropdown -->
        <label for="recipient">Select Recipient:</label>
        <select id="recipient">
            <option value="">--Select a user--</option>
        </select>

        <div id="chatBox" class="chat-box">
            <!-- Messages will be appended here -->
        </div>

        <input type="text" id="user" placeholder="Enter your name" disabled />
        <textarea id="message" placeholder="Enter your message"></textarea>
        <button id="sendMessageButton">Send</button>
    </div>

    <script src="https://ajax.aspnetcdn.com/ajax/signalr/jquery.signalR-2.4.2.min.js"></script>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>

    <script type="text/javascript">
        $(function () {
            var user = $('#user');
            var message = $('#message');
            var recipientDropdown = $('#recipient');

            // Get teacher's name from the server (using the logged-in user's session)
            $.ajax({
                type: "POST",
                url: "TeacherChat.aspx/GetTeacherName",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    user.val(data.d); // Set the user's name in the input field
                },
                error: function (error) {
                    alert('Error fetching teacher data');
                }
            });

            // Populate the recipient dropdown with users and their roles
            $.ajax({
                type: "POST",
                url: "TeacherChat.aspx/GetUsers",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d.length > 0) {
                        data.d.forEach(function (user) {
                            recipientDropdown.append('<option value="' + user.UserId + '">' + user.Name + ' (' + user.Role + ')</option>');
                        });
                    } else {
                        alert("No users found to populate the dropdown.");
                    }
                },
                error: function (error) {
                    alert('Error fetching users');
                }
            });

            // SignalR connection setup
            var connection = $.hubConnection();
            var chatHub = connection.createHubProxy('chatHub'); // Your SignalR Hub

            // Listen for messages broadcasted by the Hub
            chatHub.on('broadcastMessage', function (user, message) {
                $('#chatBox').append('<p><strong>' + user + ':</strong> ' + message + '</p>');
            });

            connection.start().done(function () {
                console.log('Connected to SignalR');
            });

            // Send message when the button is clicked
            $('#sendMessageButton').click(function () {
                var teacherName = user.val();
                var messageText = message.val();
                var recipientId = recipientDropdown.val();

                if (teacherName && messageText && recipientId) {
                    // Send message to SignalR Hub
                    chatHub.invoke('SendMessage', teacherName, messageText)
                        .done(function () {
                            saveMessageToDb(recipientId, messageText); // Save to DB
                            $('#message').val(''); // Clear message input
                        })
                        .fail(function (error) {
                            console.error('Error: ' + error);
                        });
                } else {
                    alert('Please select a recipient and enter both a name and a message.');
                }
            });

            // Save message to DB
            function saveMessageToDb(recipientId, messageText) {
                $.ajax({
                    type: "POST",
                    url: "TeacherChat.aspx/SaveMessage",
                    data: JSON.stringify({ receiverId: recipientId, message: messageText }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        console.log('Message saved to database');
                    },
                    error: function (error) {
                        console.error('Error saving message to DB: ' + error);
                    }
                });
            }
        });
    </script>
</asp:Content>
