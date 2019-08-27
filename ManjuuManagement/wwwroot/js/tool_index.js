$('#StartToWrokTime').jHsDate({
    format: 'hh:mm'
});

$('#StopToWorkTime').jHsDate({
    format: 'hh:mm'
});

$(function () {

    //增添按钮
    var addConfiguration = $('#addConfiguration');
    //更改按钮
    var alterConfiguration = $('#alterConfiguration');

    //获取表单数据
    function getFormData() {
        var formData = new Object();
        formData.StartToWrokTime = $('#StartToWrokTime').val();
        formData.StopToWorkTime = $('#StopToWorkTime').val();
        formData.PresetTimeout = $('#PresetTimeout').val() || 0;
        formData.PingSendCount = $('#PingSendCount').val() || 0;
        formData.WorkSpan = $('#WorkSpan').val();

        formData.Id = $('#Id').val() || 0;
        //if (formData.PresetTimeout != 0) {
        //    formData.PresetTimeout = parseInt(formData.PresetTimeout);
        //    if (isNaN(formData.PresetTimeout)) {
        //        formData.PresetTimeout = 0;
        //    }
        //}

        //if (formData.PingSendCount != 0) {
        //    formData.PingSendCount = parseInt(formData.PingSendCount);
        //    if (isNaN(formData.PingSendCount)) {
        //        formData.PingSendCount = 0;
        //    }
        //}


        //formData.WorkSpan = parseInt(formData.WorkSpan);
        //if (isNaN(formData.WorkSpan)) {
        //    formData.WorkSpan = '';
        //}

        return formData;

    }

    function getLoding(invoker) {
        return Ladda.create(invoker);
    }

    function notify(msg, notifyTpye, callAction) {
        msg = msg || '';
        notifyTpye = notifyTpye || 'success';
        callAction = callAction || function () { };
        new NoticeJs({
            text: msg,
            position: 'middleCenter',
            type: notifyTpye, //error， warning， info， success
            modal: true,
            animation: {
                open: 'animated lightSpeedIn',
                close: 'animated lightSpeedOut'
            },
            callbacks: {
                onClose: [callAction]
            }
        }).show();
    }


    //var Defaults = exports.Defaults = {
    //    title: '',
    //    text: '',
    //    type: 'success',
    //    position: 'topRight',
    //    timeout: 30,
    //    progressBar: true,
    //    closeWith: ['button'],
    //    animation: null,
    //    modal: false,
    //    scroll: null,
    //    callbacks: {
    //        beforeShow: [],
    //        onShow: [],
    //        afterShow: [],
    //        onClose: [],
    //        afterClose: [],
    //        onClick: [],
    //        onHover: [],
    //        onTemplate: []
    //    }
    //};

    addConfiguration.click(function () {
        //console.log(getFormData());
        //return false;
        var THIS = $(this);
        THIS.attr('disabled', 'disabled');

        var loader = getLoding(this);
        loader.start();

        axios.post(
            '/Tool/Add',
            getFormData()
        )
            .then(function (response) {
                loader.stop();
                THIS.removeAttr('disabled');
                //console.log(response);
                var serverData = response.data;
                serverData.businessResult ?
                    notify(serverData.msg, 'success', function () { location.reload(); }) :
                    notify(serverData.msg, 'error');
            })
            .catch(function (error) {
                //console.log(error);
                loader.stop();
                THIS.removeAttr('disabled');
                notify(error, 'warning');
            });

        return false;
    });


    alterConfiguration.click(function () {
        //console.log(getFormData());
        //return false;
        var THIS = $(this);
        THIS.attr('disabled', 'disabled');

        var loader = getLoding(this);
        loader.start();

        axios.post(
            '/Tool/Update',
            getFormData()
        )
            .then(function (response) {
                loader.stop();
                THIS.removeAttr('disabled');
                //console.log(response);
                var serverData = response.data;
                serverData.businessResult ?
                    notify(serverData.msg, 'success', function () { location.reload(); }) :
                    notify(serverData.msg, 'error');
            })
            .catch(function (error) {
                //console.log(error);
                loader.stop();
                THIS.removeAttr('disabled');
                notify(error, 'warning');
            });

        return false;
    });


});