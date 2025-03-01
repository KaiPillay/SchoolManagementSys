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

    <!-- Load scripts in correct order -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="/signalr/hubs"></script>
    <script src="https://ajax.aspnetcdn.com/ajax/signalr/jquery.signalR-2.4.2.min.js"></script>

    <script type="text/javascript">
        $(function () {
            var user = $('#user');
            var message = $('#message');
            var recipientDropdown = $('#recipient');
            var chatBox = $('#chatBox');

            // Get teacher's name from the server
            $.ajax({
                type: "POST",
                url: "TeacherChat.aspx/GetTeacherName",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    user.val(data.d);
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching teacher data: ', xhr.responseText);
                    alert('Error fetching teacher data. Please refresh the page.');
                }
            });

            // Populate the recipient dropdown
            $.ajax({
                type: "POST",
                url: "TeacherChat.aspx/GetUsers",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d && data.d.length > 0) {
                        data.d.forEach(function (user) {
                            recipientDropdown.append('<option value="' + user.UserId + '">' + user.Name + ' (' + user.Role + ')</option>');
                        });
                    } else {
                        alert("No users found to populate the dropdown.");
                    }
                },
                error: function (xhr, status, error) {
                    console.error('Error fetching users: ', xhr.responseText);
                    alert('Error fetching users. Please refresh the page.');
                }
            });

            // Load previous messages when a recipient is selected
            recipientDropdown.change(function () {
                var selectedRecipientId = $(this).val();
                if (selectedRecipientId) {
                    loadPreviousMessages(selectedRecipientId);
                }
            });

            function loadPreviousMessages(recipientId) {
                $.ajax({
                    type: "POST",
                    url: "TeacherChat.aspx/GetPreviousMessages",
                    data: JSON.stringify({ receiverId: parseInt(recipientId) }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        chatBox.empty(); // Clear the chat box
                        if (data.d && data.d.length > 0) {
                            data.d.forEach(function (msg) {
                                var messageClass = msg.IsSentByMe ? 'sent-message' : 'received-message';
                                chatBox.append('<p class="' + messageClass + '"><strong>' + msg.SenderName + ':</strong> ' + msg.MessageText + '</p>');
                            });
                            // Scroll to the bottom of the chat box
                            chatBox.scrollTop(chatBox[0].scrollHeight);
                        } else {
                            chatBox.append('<p class="system-message">No previous messages. Start a conversation!</p>');
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error('Error loading messages: ', xhr.responseText);
                        chatBox.append('<p class="error-message">Failed to load previous messages.</p>');
                    }
                });
            }

            // SignalR connection setup
            var connection = $.hubConnection();
            var chatHub = connection.createHubProxy('chatHub');

            // Listen for messages broadcasted by the Hub
            chatHub.on('broadcastMessage', function (senderId, senderName, receiverId, messageText) {
                // Only show message if it's relevant to the current conversation
                var currentRecipientId = recipientDropdown.val();
                if (receiverId == currentRecipientId || senderId == currentRecipientId) {
                    var messageClass = (senderId == currentRecipientId) ? 'received-message' : 'sent-message';
                    chatBox.append('<p class="' + messageClass + '"><strong>' + senderName + ':</strong> ' + messageText + '</p>');
                    chatBox.scrollTop(chatBox[0].scrollHeight);
                }
            });

            connection.start()
                .done(function () {
                    console.log('Connected to SignalR');
                })
                .fail(function (error) {
                    console.error('Failed to connect to SignalR: ', error);
                    chatBox.append('<p class="system-message">Real-time messaging unavailable. Messages will still be saved.</p>');
                });

            // Send message function
            function saveMessageToDb(recipientId, messageText) {
                $.ajax({
                    type: "POST",
                    url: "TeacherChat.aspx/SaveMessage",
                    data: JSON.stringify({
                        receiverId: parseInt(recipientId),
                        message: messageText
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d && response.d.Success) {
                            console.log('Message saved to database');
                            // Display the message in the chat box
                            chatBox.append('<p class="sent-message"><strong>' + user.val() + ':</strong> ' + messageText + '</p>');
                            chatBox.scrollTop(chatBox[0].scrollHeight);

                            // Try to broadcast via SignalR if connected
                            if (connection.state === $.signalR.connectionState.connected) {
                                chatHub.invoke('SendMessage',
                                    parseInt(response.d.SenderId),
                                    user.val(),
                                    parseInt(recipientId),
                                    messageText
                                )
                                    .fail(function (error) {
                                        console.error('SignalR broadcast error: ' + error);
                                    });
                            }
                        } else {
                            console.error('Failed to save message: ' + response.d.ErrorMessage);
                            alert('Failed to save message: ' + response.d.ErrorMessage);
                        }
                    },
                    error: function (xhr, status, error) {
                        console.error('Error saving message: ', xhr.responseText);
                        alert('Failed to save message. Please try again.');
                    }
                });
            }

            // Send button click handler
            $('#sendMessageButton').click(function () {
                var teacherName = user.val();
                var messageText = message.val().trim();
                var recipientId = recipientDropdown.val();

                if (!teacherName) {
                    alert('Teacher name not loaded. Please refresh the page.');
                    return;
                }

                if (!messageText) {
                    alert('Please enter a message.');
                    return;
                }

                if (!recipientId) {
                    alert('Please select a recipient.');
                    return;
                }

                // Save message to database
                saveMessageToDb(recipientId, messageText);

                // Clear the message input
                message.val('');
            });

            // Also allow sending with Enter key
            message.keypress(function (e) {
                if (e.which == 13 && !e.shiftKey) {
                    e.preventDefault();
                    $('#sendMessageButton').click();
                }
            });
        });
    </script>

    <style>
        .chat-container {
            max-width: 800px;
            margin: 0 auto;
            padding: 20px;
        }
        
        .chat-box {
            height: 300px;
            border: 1px solid #ccc;
            margin: 15px 0;
            padding: 10px;
            overflow-y: auto;
            background-color: #f9f9f9;
        }
        
        #message {
            width: 100%;
            height: 60px;
            margin-bottom: 10px;
            padding: 8px;
        }
        
        #user {
            width: 100%;
            margin-bottom: 10px;
            padding: 8px;
            background-color: #eee;
        }
        
        #recipient {
            width: 100%;
            padding: 8px;
            margin-bottom: 15px;
        }
        
        #sendMessageButton {
            padding: 8px 15px;
            background-color: #4CAF50;
            color: white;
            border: none;
            cursor: pointer;
        }
        
        .sent-message {
            text-align: right;
            background-color: #dcf8c6;
            padding: 5px 10px;
            border-radius: 5px;
            margin: 5px 0;
        }
        
        .received-message {
            text-align: left;
            background-color: #ffffff;
            padding: 5px 10px;
            border-radius: 5px;
            margin: 5px 0;
            border: 1px solid #e0e0e0;
        }
        
        .system-message {
            text-align: center;
            color: #888;
            font-style: italic;
        }
        
        .error-message {
            text-align: center;
            color: #ff0000;
            font-style: italic;
        }
    </style>
</asp:Content>