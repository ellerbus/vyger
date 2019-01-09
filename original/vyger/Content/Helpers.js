function setupExercisePicker(queryUrl, $group, $category, $exerciseId)
{
    $group.change(function ()
    {
        loadExercises();
    });

    $category.change(function ()
    {
        loadExercises();
    });

    loadExercises();

    function loadExercises()
    {
        var group = $group.val();
        var category = $category.val();

        var url = queryUrl + '?group=' + group + '&category=' + category;

        $.ajax({
            type: 'GET',
            url: url,
            dataType: 'json',
            success: function (exercises)
            {
                $exerciseId.find('option').remove();

                $('<option/>')
                    .attr('value', '')
                    .text('(exercise)')
                    .appendTo($exerciseId);

                $.each(exercises, function (i, exercise)
                {
                    $('<option/>')
                        .attr('value', exercise.Id)
                        .text(exercise.DetailName)
                        .appendTo($exerciseId);
                });
            }
        });
    }
}