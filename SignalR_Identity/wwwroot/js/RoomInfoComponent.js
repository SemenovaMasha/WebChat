var RoomInfoComponent = new Vue({
    el: '#room-info',
    data: {
        id: '',
        name: '',
        members: [],
        others: []
    },
    props: ['roomId'],
    methods: {
        openInfo: function (roomId) {
            $('#editAccessDenied').css('display', 'none');
            var self = this;
            axios.get('/Rooms/GetRoomInfo?roomId='+roomId)
                .then(function (response) {
                    self.id = response.data.id;
                    self.name = response.data.name;
                    self.members = response.data.members;
                    self.others = response.data.others;
                })
                .catch(function (error) {
                    alert("ERROR: " + (error.message | error));
                });
        },
        changeState: function (user) {
            var self = this;
            if (self.members.includes(user)) {
                self.remove(self.members, user);
                self.others.push(user);
            } else {
                self.remove(self.others, user);
                self.members.push(user);
            }
        },
        remove: function (array, element) {
            var index = array.indexOf(element);
            if (index > -1) {
                array.splice(index, 1);
            }
        },
        save: function () {
            var self = this;
            axios({
                method: 'post',
                url: '/Rooms/SaveChatGroup',
                headers: {},
                data: {
                    id: self.id,
                    name: self.name,
                    members: self.members
                }
            }).then(function (result) {
                    location.reload();
                }).catch(function (error) {
                    $('#editAccessDenied').css('display', 'block');
            });;
        },
    },
    created: function () {
    }
});