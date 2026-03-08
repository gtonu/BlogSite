/**
 * AdminLTE Demo Menu
 * ------------------
 * You should not use this file in production.
 * This file is for demo purposes only.
 */

/* eslint-disable camelcase */

(function ($) {
  'use strict'

  function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1)
  }

  function createSkinBlock(colors, callback, noneSelected) {
    var $block = $('<select />', {
      class: noneSelected ? 'custom-select mb-3 border-0' : 'custom-select mb-3 text-light border-0 ' + colors[0].replace(/accent-|navbar-/, 'bg-')
    })

    if (noneSelected) {
      var $default = $('<option />', {
        text: 'None Selected'
      })

      $block.append($default)
    }

    colors.forEach(function (color) {
      var $color = $('<option />', {
        class: (typeof color === 'object' ? color.join(' ') : color).replace('navbar-', 'bg-').replace('accent-', 'bg-'),
        text: capitalizeFirstLetter((typeof color === 'object' ? color.join(' ') : color).replace(/navbar-|accent-|bg-/, '').replace('-', ' '))
      })

      $block.append($color)
    })
    if (callback) {
      $block.on('change', callback)
    }

    return $block
  }

  var $sidebar = $('.control-sidebar')
  var $container = $('<div />', {
    class: 'p-3 control-sidebar-content-comment-area'
  })

  $sidebar.append($container)

  // Checkboxes

  $container.append(
    '<h5>Comments</h5><hr class="mb-2"/>'
  )

    const $commentForm = $(`
        <div class="mb-4">
            <textarea 
                class="form-control mb-2 border border-dark text-bg-grey" style="border-width: 2px !important;"
                id="new-comment-text" 
                rows="3" 
                placeholder="Write a comment...">
            </textarea>
            <button class="btn btn-primary rounded-pill btn-sm" id="post-comment">
                <i class="fa-solid fa-paper-plane" style="color: #22281f;"></i>
            </button>
            <hr />
        </div>
    `);

    /* ------------------------------
       Comment List
    ------------------------------ */
    const $commentList = $('<div />', {
        id: 'comment-list'
    });

    $container.append($commentForm, $commentList);

    const createComment = (text) => {
        return $(`
            <div class="mb-3 comment-item border-bottom pb-2">
                <strong>User</strong>
                <p class="mb-1">${text}</p>

                <a href="#" class="reply-toggle text-sm">Reply</a>

                <div class="reply-area d-none mt-2">
                    <textarea 
                        class="form-control mb-2 reply-text" 
                        rows="2" 
                        placeholder="Write a reply...">
                    </textarea>
                    <button class="btn btn-secondary btn-sm post-reply">
                        Reply
                    </button>
                </div>

                <div class="replies mt-2 pl-3"></div>
            </div>
        `);
    };

    const createReply = (text) => {
        return $(`
            <div class="text-sm mb-1">
                <strong>User</strong> ${text}
            </div>
        `);
    };

    /* ------------------------------
       Events
    ------------------------------ */

    // Post new comment
    $(document).on('click', '#post-comment', function () {
        const text = $('#new-comment-text').val().trim();
        if (!text) return;

        $commentList.prepend(createComment(text));
        $('#new-comment-text').val('');
    });

    // Toggle reply box
    $(document).on('click', '.reply-toggle', function (e) {
        e.preventDefault();
        $(this).siblings('.reply-area').toggleClass('d-none');
    });

    // Post reply
    $(document).on('click', '.post-reply', function () {
        const $comment = $(this).closest('.comment-item');
        const text = $comment.find('.reply-text').val().trim();
        if (!text) return;

        $comment.find('.replies').append(createReply(text));
        $comment.find('.reply-text').val('');
        $comment.find('.reply-area').addClass('d-none');
    });

})(jQuery);
