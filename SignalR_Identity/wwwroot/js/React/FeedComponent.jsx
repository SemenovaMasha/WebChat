
class FeedComponent extends React.Component {
    constructor(props) {
        super(props);
        
        this.addNewPost = this.addNewPost.bind(this);
        this.state = {
            userId: props.userId,
            currentUsername: props.currentUsername,
            isAuth: props.isAuth
        }
    }

    render() {
        if (this.state.isAuth) {
            form = <PostForm
                userId={this.state.userId}
                addHandle={this.addNewPost}
                isAuth={this.state.isAuth}
                currentUsername={this.state.currentUsername} />;
        } else {
            form = <div></div>;
        }
        return (
            <div >
                <h2>Feed</h2>
                {form}
                <PostsList userId={this.state.userId} ref="postsListRef"/>
        
            </div>
        );
    }
    
    addNewPost(post) {
        this.refs.postsListRef.addNewPost(post);
    }
}
