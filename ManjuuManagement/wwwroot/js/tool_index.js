$('#StartToWrokTime').jHsDate({
    format: 'hh:mm'
});

$('#StopToWorkTime').jHsDate({
    format: 'hh:mm'
});

$(function(){

    //增添按钮
    var addConfiguration = $('#addConfiguration');
    //更改按钮
    var alterConfiguration = $('#alterConfiguration');

    //获取表单数据
    function getFormData(){
            var formData = new Object();
            formData.StartToWrokTime = $('#StartToWrokTime').val();
            formData.StopToWorkTime = $('#StopToWorkTime').val();
            formData.PresetTimeout = $('#PresetTimeout').val();
            formData.PingSendCount = $('#PingSendCount').val();
            formData.WorkSpan = $('#WorkSpan').val();

            formData.Id = $('#Id').val() || 0;

            return formData;    

    }

    
    addConfiguration.click(function(){
        $(this).attr('disabled','disabled');

        axios.post(
            '/Tool/Add',
            getFormData()
        )
        .then(function(response){
            console.log(response);
        })
        .catch(function(error){
            console.log(error);
        });
    });



});