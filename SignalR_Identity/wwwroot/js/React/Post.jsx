
class Post extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            id: props.id,
            text: props.text,
            date: moment(props.date, "YYYY-MM-DDTHH:mm:SSS").format("HH:mm DD.MM.YY"),
            liked: props.liked,
            isOwner: props.isOwner,
            userAuth: props.userAuth,
            authorAvatarPath: props.authorAvatarPath,
            authorName: props.authorName,
            removeHandler: props.removeHandler
        }

        this.changeLike = this.changeLike.bind(this);
        this.deletePost = this.deletePost.bind(this);
    }

    changeLike(e) {
        e.preventDefault();

        this.setState({
            liked: !this.state.liked
        });

        fetch('https://localhost:44358/api/Posts/ChangeLike?postId='+this.state.id,
                {
                    method: 'POST'
                });
    }

    deletePost(e) {
        e.preventDefault();
        
        this.state.removeHandler(this.state.id);
        
        fetch('https://localhost:44358/api/Posts/DeletePost?postId=' + this.state.id,
                {
                    method: 'DELETE'
                })
            .then(result => result.json())
            .then(deleted => {
                //if (deleted)
                //    this.state.removeHandler(this.state.id);
            });
    }

    render() {
        if (this.state.userAuth) {
            likeSection = <a href="#" onClick={(e) => this.changeLike(e)}> 
                    <svg width="100%" height="100%" xmlns="http://www.w3.org/2000/svg" xlink="http://www.w3.org/1999/xlink"
                        {...(this.state.liked &&
                        {
                            viewBox:"0 0 20 20",
                            fill:"#ff492f",
                            stroke:"none"
                            })}
                        {...(!this.state.liked &&
                        {
                            viewBox:"-2 -2 24 24",
                            fill:"none",
                            stroke:"#8f8f8f",
                            strokeWidth: "2"
                        })}
                    ><path d="M14.0425097,0.000920262799 C16.1435097,-0.0400797372 18.8835097,1.28192026 18.9635097,5.36992026 C19.0525097,9.95492026 15.1985097,13.3079203 9.48350967,16.2089203 C3.76650967,13.3079203 -0.0874903349,9.95492026 0.00150966509,5.36992026 C0.0815096651,1.28192026 2.82150967,-0.0400797372 4.92250967,0.000920262799 C7.02450967,0.0419202628 8.87050967,2.26592026 9.46950967,2.92792026 C10.0945097,2.26592026 11.9405097,0.0419202628 14.0425097,0.000920262799 Z"></path></svg>
                              </a>;
        } else {
            likeSection = <div></div>;
        }
        if (this.state.isOwner) {
            deleteSection = <a href="#" onClick={(e) => this.deletePost(e)}>
                <i className="fa fa-remove"></i>
            </a>;
        } else {
            deleteSection = <div></div>;
        }
        avatarPath = '/images/avatars' + this.state.authorAvatarPath ;
        return (
            <li className="list-group-item">
                <div className="row postRow postHeader">
                    <img src={avatarPath} className="img-rounded img-responsive"/>
                    <p>{this.state.authorName}</p>
                    {deleteSection}
                </div>
                <div className="row postRow postContainer">
                    <p>{this.state.text}</p>
                </div>
                <div className="row postRow postBottom">
                    <div>{this.state.date}</div>
                    {likeSection}
                </div>
            </li>
        );
    }
}